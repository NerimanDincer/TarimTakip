namespace TarimTakip.API.Data.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public int ChatRoomId { get; set; } // Foreign Key
        public int SenderId { get; set; } // Foreign Key (User tablosuna)
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public virtual ChatRoom ChatRoom { get; set; }
        public virtual User Sender { get; set; }
    }
}