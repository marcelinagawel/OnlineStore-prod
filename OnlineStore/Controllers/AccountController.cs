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
            var user = _context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);

            if (user != null)
            {
                // Ustaw ciasteczka z UserId i is_admin
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
                    Response.Cookies.Delete("is_admin"); // Usuń ciasteczko, jeśli nie jest administratorem
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

        // Rejestracja użytkownika
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User newUser)
        {
            if (ModelState.IsValid)
            {
                // Ustawianie domyślnych wartości
                newUser.IsAdmin = false; // Nowi użytkownicy nie są administratorami
                _context.Users.Add(newUser);
                _context.SaveChanges();

                // Automatyczne logowanie po rejestracji
                Response.Cookies.Append("UserId", newUser.Id.ToString(), new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTime.Now.AddMinutes(30)
                });

                return RedirectToAction("Details");
            }

            return View(newUser);
        }
    }
}
