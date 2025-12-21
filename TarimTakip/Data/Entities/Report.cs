using System.ComponentModel.DataAnnotations.Schema;

namespace TarimTakip.API.Data.Entities
{
    public class Report
    {
        public int Id { get; set; }

        public string Reason { get; set; } // Örn: "Uygunsuz resim", "Dolandırıcı", "Yanlış fiyat"
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // --- İlişkiler ---

        // Hangi İlan Şikayet Edildi?
        public int AdvertId { get; set; }
        [ForeignKey("AdvertId")]
        public virtual Advert Advert { get; set; }

        // Kim Şikayet Etti? (Gizli tanık)
        public int ReporterId { get; set; }
        [ForeignKey("ReporterId")]
        public virtual User Reporter { get; set; }
    }
}