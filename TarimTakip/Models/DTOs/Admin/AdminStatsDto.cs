namespace TarimTakip.API.Models.DTOs.Admin
{
    public class AdminStatsDto
    {
        public int TotalFarmers { get; set; }
        public int TotalEngineers { get; set; }
        public int TotalUsers { get; set; }
        public int PendingQuestions { get; set; }
        public decimal TotalExpensesThisMonth { get; set; }
        public decimal TotalIncomeThisMonth { get; set; }
    }
}
