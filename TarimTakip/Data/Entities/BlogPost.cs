namespace TarimTakip.API.Data.Entities
{
    public class BlogPost
    {
        public int Id { get; set; }
        public int EngineerId { get; set; } // Foreign Key (User tablosuna)
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Property
        public virtual User Engineer { get; set; }
    }
}
