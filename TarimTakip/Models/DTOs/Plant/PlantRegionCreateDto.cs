using System.ComponentModel.DataAnnotations;

namespace TarimTakip.API.Models.DTOs.Plant
{
    public class PlantRegionCreateDto
    {
        [Required]
        public int PlantId { get; set; } // Hangi bitkiye (örn: 1 - Buğday)

        [Required]
        public int RegionId { get; set; } // Hangi bölgeye (örn: 1002 - Adana)

        public DateTime? PlantingTimeStart { get; set; } // O bölgeye özel ekim başı
        public DateTime? PlantingTimeEnd { get; set; } // O bölgeye özel ekim sonu
        public string? RegionSpecificNotes { get; set; } // O bölgeye özel notlar
    }
}