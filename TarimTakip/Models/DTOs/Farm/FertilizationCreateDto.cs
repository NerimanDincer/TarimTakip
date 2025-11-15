using System.ComponentModel.DataAnnotations;

namespace TarimTakip.API.Models.DTOs.Farm
{
    public class FertilizationCreateDto
    {
        public string? Description { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }
}