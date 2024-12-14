using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Proszę podać nazwę kategorii.")]
        [StringLength(100, ErrorMessage = "Nazwa kategorii nie może być dłuższa niż 100 znaków.")]
        public string Name { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
