using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OnlineStore.Models;

namespace OnlineStore.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // Strona g³ówna
        public IActionResult Index()
        {
            // Pobierz 5 najnowszych produktów
            var latestProducts = _context.Products
                .OrderByDescending(p => p.Id)
                .Take(5)
                .ToList();

            return View(latestProducts);
        }

        // Wszystkie produkty
        public IActionResult AllProducts()
        {
            // Pobierz wszystkie produkty
            var products = _context.Products.ToList();
            return View(products);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public int GetCartItemCount()
        {
            var cartCookie = Request.Cookies["Cart"];
            if (string.IsNullOrEmpty(cartCookie))
            {
                return 0;
            }

            var cart = JsonConvert.DeserializeObject<List<CartItem>>(cartCookie);
            Console.WriteLine("fsdf");
            return cart.Sum(c => c.Quantity);
        }
    }
}
