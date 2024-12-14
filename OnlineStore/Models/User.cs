using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
       
        public string Email { get; set; }

        [Required]
       
      
        public string Password { get; set; }

        [Required]
        public bool IsAdmin { get; set; } // Nowe pole: Czy użytkownik jest administratorem

        public ICollection<Order> Orders { get; set; } // Zamówienia użytkownika

    }
}
