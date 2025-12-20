namespace TarimTakip.API.Data.Entities
{
    public class Question
    {
        public int Id { get; set; }
        public int FarmerId { get; set; } // Foreign Key (User tablosuna)
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } // "Pending", "Answered"

        // Navigation Properties
        public virtual User Farmer { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
        public string? ImageUrl { get; set; } // Resim dosyası değil, sadece yolu (adres) tutulur
    }
}
