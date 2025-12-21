using System.ComponentModel.DataAnnotations.Schema;

namespace TarimTakip.API.Data.Entities
{
    public class Advert
    {
        public int Id { get; set; }

        public string Title { get; set; } // Örn: "Satılık Domates Fidesi"
        public string Description { get; set; } // "Çok temiz, yerli tohum..."
        public decimal Price { get; set; } // 500.00 TL

        // Opsiyonel: Birim (Kg, Ton, Adet)
        public string Unit { get; set; } = "Adet"; // Varsayılan Adet olsun
        public int Quantity { get; set; } // Kaç tane var?

        public string? ImageUrl { get; set; } // Ürünün resmi

        public bool IsActive { get; set; } = true; // Satılınca veya kaldırılınca False olur
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // --- İlişkiler ---

        // İlanı veren Çiftçi (User tablosuna bağlanacak)
        public int SellerId { get; set; }

        [ForeignKey("SellerId")]
        public virtual User Seller { get; set; }
    }
}