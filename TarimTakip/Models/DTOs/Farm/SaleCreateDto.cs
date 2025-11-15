using System.ComponentModel.DataAnnotations;

namespace TarimTakip.API.Models.DTOs.Farm
{
    public class SaleCreateDto
    {
        [Required]
        public decimal AmountKg { get; set; } // Satılan Miktar (Kg)

        [Required]
        public decimal Price { get; set; } // Elde Edilen Toplam Gelir (Fiyat)

        [Required]
        public DateTime Date { get; set; }
    }
}