namespace TarimTakip.API.Data.Entities
{
    public class PlantInfo
    {
        public int Id { get; set; }
        public string PlantName { get; set; }
        public string? SoilPreparation { get; set; }
        public string? WaterNeeds { get; set; }
        public string? HarvestTime { get; set; }

        // Navigation Properties (Many-to-Many için)
        public virtual ICollection<PlantRegion> PlantRegions { get; set; }
    }
}