using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Models
{
   public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Proszę podać nazwę produktu.")]
        [StringLength(100, ErrorMessage = "Nazwa produktu nie może być dłuższa niż 100 znaków.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Proszę podać cenę produktu.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Cena produktu musi być większa niż 0.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Proszę podać ilość.")]
        [Range(0.01, int.MaxValue, ErrorMessage = "Ilość musi być większa niż 0.")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Proszę wybrać kategorię.")]
        public int CategoryId { get; set; } // Powiązanie z kategorią

        [Required(ErrorMessage = "Proszę podać link do zdjęcia produktu.")]
        [Url(ErrorMessage = "Proszę podać prawidłowy adres URL do zdjęcia.")]
        public string Image { get; set; } // Link do zdjęcia

        public Category Category { get; set; } // Nawigacja
    }
}
