namespace TarimTakip.API.Models.DTOs.Report
{
    public class FarmFieldReportDto
    {
        public int FarmFieldId { get; set; }
        public string PlantName { get; set; }
        public decimal TotalExpenses { get; set; } // Toplam Masraf
        public decimal TotalIncome { get; set; } // Toplam Gelir
        public decimal NetProfit { get; set; } // Net Kar (Gelir - Gider)
    }
}