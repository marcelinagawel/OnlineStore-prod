using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using OnlineStore.Models;

namespace OnlineStore.Controllers
{
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            // Ustaw liczbę produktów w koszyku
            ViewData["CartItemCount"] = GetCartItemCount();

            // Ustaw informacje o zalogowanym użytkowniku
            var userId = Request.Cookies["UserId"];
            ViewData["IsLoggedIn"] = !string.IsNullOrEmpty(userId);
            ViewData["UserId"] = userId;
        }

        private int GetCartItemCount()
        {
            var cartCookie = Request.Cookies["Cart"];
            if (string.IsNullOrEmpty(cartCookie))
            {
                return 0;
            }

            var cart = JsonConvert.DeserializeObject<List<CartItem>>(cartCookie);
            return cart.Sum(c => c.Quantity);
        }
    }
}
