using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        public DateTime OrderDate { get; set; } // Data zamówienia

        [Required]
        public int UserId { get; set; } // ID użytkownika, który złożył zamówienie

        public User User { get; set; }

        public ICollection<OrderProduct> OrderProducts { get; set; } // Powiązane produkty
    }
}
