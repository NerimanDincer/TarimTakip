namespace TarimTakip.API.Data.Entities
{
    public class ChatRoom
    {
        public int Id { get; set; }
        public int FarmerId { get; set; } // Foreign Key (User tablosuna)
        public int EngineerId { get; set; } // Foreign Key (User tablosuna)
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public virtual User Farmer { get; set; }
        public virtual User Engineer { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
    }
}