namespace TarimTakip.API.Models.DTOs.Report
{
    public class ReportResponseDto
    {
        public int Id { get; set; } // Raporun ID'si (Bunu silmek için kullanacağız)
        public string Reason { get; set; } // Şikayet sebebi
        public DateTime CreatedAt { get; set; }

        // Şikayet Edilen İlan Bilgileri
        public int AdvertId { get; set; }
        public string AdvertTitle { get; set; }

        // İspiyonlayan Kişi :)
        public string ReporterName { get; set; }
        public string SellerName { get; set; } // Şikayet Edilen (Satıcı)
    }
}