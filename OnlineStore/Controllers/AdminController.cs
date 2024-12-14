using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Models;
using System.Diagnostics;

namespace OnlineStore.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Metoda sprawdzająca uprawnienia admina
        private void CheckAdmin()
        {
            if (Request.Cookies["is_admin"] != "true")
            {
                Response.Redirect("/Admin/Login");
            }
        }

        // Formularz logowania
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // Obsługa logowania
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var admin = _context.Users
                .FirstOrDefault(u => u.Email == email && u.Password == password);

            if (admin != null)
            {
                if (admin.IsAdmin)
                {
                    Response.Cookies.Append("is_admin", "true", new CookieOptions
                    {
                        HttpOnly = true,
                        Expires = DateTime.Now.AddMinutes(30)
                    });
                }

                
                Response.Cookies.Append("UserId", admin.Id.ToString(), new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTime.Now.AddMinutes(30)
                });

                return RedirectToAction("Index");
            }

            ViewBag.Error = "Nieprawidłowe dane logowania.";
            return View();
        }

        // Wylogowanie
        public IActionResult Logout()
        {
            Response.Cookies.Delete("is_admin");
            Response.Cookies.Delete("UserId");
            Response.Cookies.Delete("Cart");
            return RedirectToAction("Login");
        }

        // Główna strona admina
        public IActionResult Index()
        {
            Console.WriteLine("test");
            CheckAdmin();
            return View(); // Widok: Views/Admin/Index.cshtml
        }

        // --------------- Produkty ---------------
        public async Task<IActionResult> Products()
        {
            Console.WriteLine("test");
            CheckAdmin();
            var products = await _context.Products.Include(p => p.Category).ToListAsync();
            return View(products); // Widok: Views/Admin/Products.cshtml
        }

        public IActionResult CreateProduct()
        {
            CheckAdmin();
            ViewBag.Categories = _context.Categories
            .Select(c => new SelectListItem
            {
                Value = c.Id.ToString(), // Wartość powinna być typu string, ale odpowiadająca int
                Text = c.Name
            })
            .ToList();
            return View(); // Widok: Views/Admin/CreateProduct.cshtml
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(Product product)
        {
            CheckAdmin();

            
            ModelState.Remove("CategoryId");
            ModelState.Remove("Category");
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _context.Categories
                    .Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Name
                    })
                    .ToList();

                return View(product);
            }

            _context.Products.Add(product); // utrwalenie danych EF
            await _context.SaveChangesAsync();

            return RedirectToAction("Products");
        }

        public async Task<IActionResult> EditProduct(int id)
        {
            CheckAdmin();
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            ViewBag.Categories = _context.Categories
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                })
                .ToList();
            return View(product); // Widok: Views/Admin/EditProduct.cshtml
        }


        [HttpPost]
        public async Task<IActionResult> EditProduct(int id, Product product)
        {
            CheckAdmin();

            
            if (id != product.Id)
            {
                return BadRequest("Nieprawidłowe ID produktu.");
            }

        
            ModelState.Remove("Category");

            if (!ModelState.IsValid)
            {
                
                ViewBag.Categories = _context.Categories
                    .Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Name
                    })
                    .ToList();

                return View(product); 
            }

            try
            {
             
                _context.Entry(product).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return RedirectToAction("Products");
            }
            catch (DbUpdateConcurrencyException)
            {
             
                if (!_context.Products.Any(p => p.Id == id))
                {
                    return NotFound("Nie znaleziono produktu o podanym ID.");
                }
                else
                {
                    throw; // Rzuć wyjątek dla innych błędów
                }
            }
        }


        public async Task<IActionResult> DeleteProduct(int id)
        {
            CheckAdmin();

            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction("Products");
        }

        // --------------- Kategorie ---------------
        public async Task<IActionResult> Categories()
        {
            CheckAdmin();
            var categories = await _context.Categories.ToListAsync();
            return View(categories);
        }

        public IActionResult CreateCategory()
        {
            CheckAdmin();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(Category category)
        {
            CheckAdmin();

            ModelState.Remove("Products");

            if (!ModelState.IsValid)
            {
                return View(category);
            }

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return RedirectToAction("Categories");
            
        
        }

        public async Task<IActionResult> EditCategory(int id)
        {
            CheckAdmin();
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound();

            
            return View(category); // Widok: Views/Admin/EditProduct.cshtml
        }

        [HttpPost]
        public async Task<IActionResult> EditCategory(int id, Category category)
        {
            CheckAdmin();

            ModelState.Remove("Products");

            if (!ModelState.IsValid)
            {
                return View(category);
            }

            if (id != category.Id)
            {
                return BadRequest("Nieprawidłowe ID produktu.");
            }

            _context.Entry(category).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return RedirectToAction("Categories");
        }

        public async Task<IActionResult> DeleteCategory(int id)
        {
            CheckAdmin();

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return RedirectToAction("Categories");
        }

        // --------------- Zamówienia ---------------
        // Lista zamówień
        public async Task<IActionResult> Orders()
        {
            CheckAdmin();
            var orders = await _context.Orders.Include(o => o.User).ToListAsync();
            return View(orders); // Widok: Views/Admin/Orders.cshtml
        }

        // Szczegóły konkretnego zamówienia
        public async Task<IActionResult> OrderDetails(int id)
        {
            CheckAdmin();

            var order = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound("Zamówienie nie zostało znalezione.");
            }

            return View(order); // Widok: Views/Admin/OrderDetails.cshtml
        }
    }
}
