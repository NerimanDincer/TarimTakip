using System.ComponentModel.DataAnnotations;

namespace TarimTakip.API.Models.DTOs.Plant
{
    public class PlantInfoCreateDto
    {
        [Required]
        public string PlantName { get; set; }
        public string? SoilPreparation { get; set; }
        public string? WaterNeeds { get; set; }
        public string? HarvestTime { get; set; }
    }
}
