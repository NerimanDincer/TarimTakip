namespace TarimTakip.API.Data.Entities
{
    public class Sale
    {
        public int Id { get; set; }
        public int FarmFieldId { get; set; } // Foreign Key
        public decimal AmountKg { get; set; }
        public decimal Price { get; set; } // (Birim fiyat veya toplam fiyat, sana bağlı)
        public DateTime Date { get; set; }

        // Navigation Property
        public virtual FarmField FarmField { get; set; }
    }
}