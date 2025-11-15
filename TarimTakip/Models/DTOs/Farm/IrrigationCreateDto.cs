using System.ComponentModel.DataAnnotations;

namespace TarimTakip.API.Models.DTOs.Farm
{
    public class IrrigationCreateDto
    {
        [Required]
        public decimal LitersUsed { get; set; } // Kullanılan Su (Litre)

        public string? Description { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }
}