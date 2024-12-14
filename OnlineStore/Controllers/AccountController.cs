using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Models;

namespace OnlineStore.Controllers
{
    public class AccountController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Widok logowania
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // Obsługa logowania
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var errors = new List<string>();

            // Ręczna walidacja
            if (string.IsNullOrEmpty(email))
            {
                errors.Add("Proszę podać adres e-mail.");
            }
            else if (!email.Contains("@") || !email.Contains("."))
            {
                errors.Add("Nieprawidłowy format adresu e-mail.");
            }

            if (string.IsNullOrEmpty(password))
            {
                errors.Add("Proszę podać hasło.");
            }
            else if (password.Length < 5)
            {
                errors.Add("Hasło musi mieć co najmniej 5 znaków.");
            }

            if (errors.Any())
            {
                ViewBag.ValidationErrors = errors;
                return View();
            }

            // Logika logowania
            var user = _context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);

            if (user != null)
            {
                Response.Cookies.Append("UserId", user.Id.ToString(), new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTime.Now.AddMinutes(30)
                });

                if (user.IsAdmin)
                {
                    Response.Cookies.Append("is_admin", "true", new CookieOptions
                    {
                        HttpOnly = true,
                        Expires = DateTime.Now.AddMinutes(30)
                    });
                }
                else
                {
                    Response.Cookies.Delete("is_admin");
                }

                return RedirectToAction("Details");
            }

            ViewBag.Error = "Nieprawidłowy email lub hasło.";
            return View();
        }


        // Wylogowanie
        public IActionResult Logout()
        {
            Response.Cookies.Delete("UserId");
            Response.Cookies.Delete("is_admin");
            Response.Cookies.Delete("Cart"); // Czyść koszyk
            return RedirectToAction("Login");
        }

        // Szczegóły konta z zamówieniami
        public IActionResult Details()
        {
            var userIdString = Request.Cookies["UserId"];
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return RedirectToAction("Login");
            }

            var user = _context.Users
                .Include(u => u.Orders)
                .ThenInclude(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
                .FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                return RedirectToAction("Login");
            }

            return View(user);
        }
    }
}
