using System.ComponentModel.DataAnnotations;

namespace TarimTakip.API.Models.DTOs.Advert
{
    public class AdvertCreateDto
    {
        public string Title { get; set; }       // "Köy Domatesi"
        public string Description { get; set; } // "İlaçsız, doğal..."
        public decimal Price { get; set; }      // 50.00
        public string Unit { get; set; } = "Kg"; // Kg, Ton, Kasa
        public int Quantity { get; set; }       // 500

        [Required(ErrorMessage = "Ürün resmi yüklemek zorunludur!")]
        public string ImageUrl { get; set; }
    }
}