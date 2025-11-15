namespace TarimTakip.API.Models.DTOs.Report
{
    public class GeneralReportDto
    {
        public List<FarmFieldReportDto> FarmFieldReports { get; set; }
        public decimal OverallTotalExpenses { get; set; } // Tüm tarlaların toplam masrafı
        public decimal OverallTotalIncome { get; set; } // Tüm tarlaların toplam geliri
        public decimal OverallNetProfit { get; set; } // Toplam Net Kar
    }
}