namespace TarimTakip.API.Models.DTOs.Plant
{
    public class PlantInfoResponseDto
    {
        public int Id { get; set; }
        public string PlantName { get; set; }
        public string? SoilPreparation { get; set; }
        public string? WaterNeeds { get; set; }
        public string? HarvestTime { get; set; }
    }
}