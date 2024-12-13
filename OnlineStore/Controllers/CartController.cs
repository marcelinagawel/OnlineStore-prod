using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OnlineStore.Models;

namespace OnlineStore.Controllers
{
    public class CartController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Dodawanie do koszyka
        public IActionResult AddToCart(int productId, int qty = 1)
        {
            // Pobierz produkt z bazy danych
            var product = _context.Products.Find(productId);
            if (product == null)
            {
                return NotFound();
            }

            // Pobierz koszyk z ciasteczka
            var cart = GetCart();

            // Sprawdź, czy produkt już istnieje w koszyku
            var cartItem = cart.FirstOrDefault(c => c.ProductId == productId);
            if (cartItem == null)
            {
                // Dodaj nowy produkt do koszyka
                cart.Add(new CartItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    ProductImage = product.Image,
                    ProductPrice = product.Price,
                    Quantity = qty
                });
            }
            else
            {
                // Zwiększ ilość
                cartItem.Quantity += qty;
            }

            // Zapisz koszyk do ciasteczka
            SaveCart(cart);

            return RedirectToAction("Index", "Home");
        }

        // Wyświetlanie koszyka
        public IActionResult Index()
        {
            var cart = GetCart();
            return View(cart);
        }

        // Usuwanie z koszyka
        public IActionResult RemoveFromCart(int productId)
        {
            var cart = GetCart();
            var cartItem = cart.FirstOrDefault(c => c.ProductId == productId);
            if (cartItem != null)
            {
                cart.Remove(cartItem);
            }

            SaveCart(cart);

            return RedirectToAction("Index");
        }

        // Aktualizacja ilości
        public IActionResult UpdateQuantity(int productId, int qty)
        {
            var cart = GetCart();
            var cartItem = cart.FirstOrDefault(c => c.ProductId == productId);
            if (cartItem != null)
            {
                cartItem.Quantity = qty > 0 ? qty : 1; // Minimalna ilość to 1
            }

            SaveCart(cart);

            return RedirectToAction("Index");
        }

        // Metody pomocnicze
        private List<CartItem> GetCart()
        {
            var cookie = Request.Cookies["Cart"];
            return string.IsNullOrEmpty(cookie)
                ? new List<CartItem>()
                : JsonConvert.DeserializeObject<List<CartItem>>(cookie);
        }

        private void SaveCart(List<CartItem> cart)
        {
            var options = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(7), // Ciasteczko ważne przez 7 dni
                HttpOnly = true
            };

            var cartJson = JsonConvert.SerializeObject(cart);
            Response.Cookies.Append("Cart", cartJson, options);
        }

        public IActionResult Checkout()
        {
            var cart = GetCart(); // Pobierz dane koszyka

            if (!cart.Any())
            {
                return RedirectToAction("Index"); // Jeśli koszyk pusty, wróć do koszyka
            }

            // Sprawdź użytkownika (z ciasteczka)
            var userIdString = Request.Cookies["UserId"];
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return RedirectToAction("Login", "Account"); // Brak użytkownika - przekieruj do logowania
            }

            // Tworzenie zamówienia
            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                OrderProducts = cart.Select(c => new OrderProduct
                {
                    ProductId = c.ProductId,
                    Quantity = c.Quantity
                }).ToList()
            };

            _context.Orders.Add(order);
            _context.SaveChanges(); // Zapisz zamówienie i produkty

            // Wyczyść koszyk
            Response.Cookies.Delete("Cart");

            return RedirectToAction("OrderConfirmation", new { orderId = order.Id });
        }

        public IActionResult OrderConfirmation(int orderId)
        {
            var order = _context.Orders
                .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
                .FirstOrDefault(o => o.Id == orderId);

            if (order == null)
            {
                return NotFound("Zamówienie nie zostało znalezione.");
            }

            return View(order); // Widok: Views/Cart/OrderConfirmation.cshtml
        }


    }


}
