namespace TarimTakip.API.Data.Entities
{
    public class CalendarNote
    {
        public int Id { get; set; }
        public int UserId { get; set; } // Foreign Key (Kime ait)
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime Date { get; set; }

        // Navigation Property
        public virtual User User { get; set; }
    }
}