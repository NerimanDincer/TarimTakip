namespace TarimTakip.API.Models.DTOs.Plant
{
    public class PlantRegionResponseDto
    {
        public int Id { get; set; } // Bu 'PlantRegion' kaydının kendi ID'si
        public int PlantId { get; set; }
        public string PlantName { get; set; }
        public int RegionId { get; set; }
        public string RegionName { get; set; }
        public DateTime? PlantingTimeStart { get; set; }
        public DateTime? PlantingTimeEnd { get; set; }
        public string? RegionSpecificNotes { get; set; }
    }
}
