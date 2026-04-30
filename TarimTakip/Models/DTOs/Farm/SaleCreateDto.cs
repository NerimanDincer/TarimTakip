using System.ComponentModel.DataAnnotations;

namespace TarimTakip.API.Models.DTOs.Farm
{
    public class SaleCreateDto
    {
        [Required]
        public decimal AmountKg { get; set; }

        [Required]
        public decimal UnitPrice { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }
}