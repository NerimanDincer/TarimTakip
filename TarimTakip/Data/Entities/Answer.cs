namespace TarimTakip.API.Data.Entities
{
    public class Answer
    {
        public int Id { get; set; }
        public int QuestionId { get; set; } // Foreign Key
        public int EngineerId { get; set; } // Foreign Key (User tablosuna)
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public virtual Question Question { get; set; }
        public virtual User Engineer { get; set; }
    }
}