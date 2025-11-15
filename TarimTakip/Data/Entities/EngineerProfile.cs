namespace TarimTakip.API.Data.Entities
{
    public class EngineerProfile
    {
        public int Id { get; set; }
        public int UserId { get; set; } // Foreign Key (User ile 1'e 1 ilişki)
        public string? University { get; set; }
        public int ExperienceYears { get; set; }
        public string? Expertise { get; set; }
        public string? Certificates { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Property
        public virtual User User { get; set; }
    }
}