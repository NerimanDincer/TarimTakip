namespace TarimTakip.API.Models.DTOs.Farm
{
    public class FarmFieldDetailDto
    {
        public int Id { get; set; }
        public string PlantName { get; set; }
        public DateTime SowingDate { get; set; }
        public decimal Area { get; set; }
        public string? SoilInfo { get; set; }
        public string RegionName { get; set; }

        // İlişkili listeler
        public List<ExpenseResponseDto> Expenses { get; set; }
        public List<IrrigationResponseDto> Irrigations { get; set; }
        public List<FertilizationResponseDto> Fertilizations { get; set; }
    }
}