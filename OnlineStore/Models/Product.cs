using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nazwa produktu jest wymagana.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Cena produktu jest wymagana.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Cena musi być większa niż 0.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Ilość jest wymagana.")]
        [Range(0, int.MaxValue, ErrorMessage = "Ilość musi być większa lub równa 0.")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Kategoria jest wymagana.")]
        public int CategoryId { get; set; } // Powiązanie z kategorią

        [Required(ErrorMessage = "Zdjęcie produktu jest wymagane.")]
        [Url(ErrorMessage = "Podaj prawidłowy link do zdjęcia.")]
        public string Image { get; set; } // Link do zdjęcia

        public Category Category { get; set; } // Nawigacja
    }
}
