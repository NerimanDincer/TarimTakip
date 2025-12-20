namespace TarimTakip.API.Data.Entities
{
    public class Region
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // Navigation Properties
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<FarmField> FarmFields { get; set; }
        public virtual ICollection<PlantRegion> PlantRegions { get; set; }
        public double Latitude { get; set; }  // Enlem (Örn: 36.88)
        public double Longitude { get; set; } // Boylam (Örn: 30.70)
    }
}