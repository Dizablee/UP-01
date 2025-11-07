using System.ComponentModel.DataAnnotations;

namespace EuroLink.Models
{
    public class Phone  
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Бренд обязателен")]
        public string Brand { get; set; } = string.Empty;

        [Required(ErrorMessage = "Модель обязательна")]
        public string Model { get; set; } = string.Empty;

        [Required(ErrorMessage = "Цена обязательна")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Цена должна быть больше 0")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Количество обязательно")]
        [Range(0, int.MaxValue, ErrorMessage = "Количество не может быть отрицательным")]
        public int StockQuantity { get; set; }

        public string? Color { get; set; }
        public string? Memory { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
    }
}