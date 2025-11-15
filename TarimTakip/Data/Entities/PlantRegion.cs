namespace TarimTakip.API.Data.Entities
{
    public class PlantRegion
    {
        public int Id { get; set; }
        public int PlantId { get; set; } // Foreign Key
        public int RegionId { get; set; } // Foreign Key

        public DateTime? PlantingTimeStart { get; set; }
        public DateTime? PlantingTimeEnd { get; set; }
        public string? RegionSpecificNotes { get; set; }

        // Navigation Properties
        public virtual PlantInfo PlantInfo { get; set; }
        public virtual Region Region { get; set; }
    }
}