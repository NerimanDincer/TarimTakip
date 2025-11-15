namespace TarimTakip.API.Models.DTOs.Farm
{
    public class ExpenseResponseDto
    {
        public int Id { get; set; }
        public string CostType { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string? Note { get; set; }
    }
}