namespace TarimTakip.API.Data.Entities
{
    public class Irrigation
    {
        public int Id { get; set; }
        public int FarmFieldId { get; set; } // Foreign Key
        public decimal LitersUsed { get; set; }
        public string? Description { get; set; }
        public DateTime Date { get; set; }

        // Navigation Property
        public virtual FarmField FarmField { get; set; }
    }
}