namespace TarimTakip.API.Models.DTOs.Farm
{
    public class IrrigationResponseDto
    {
        public int Id { get; set; }
        public decimal LitersUsed { get; set; }
        public string? Description { get; set; }
        public DateTime Date { get; set; }
    }
}