using TarimTakip.Data.Entities;

namespace TarimTakip.API.Data.Entities
{
    public class FarmField : BaseEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; } // Foreign Key (Sahibi)
        public int RegionId { get; set; } // Foreign Key (Konumu)
        public string PlantName { get; set; }
        public DateTime SowingDate { get; set; }
        public decimal Area { get; set; }
        public string? SoilInfo { get; set; }

        // Navigation Properties
        public virtual User User { get; set; }
        public virtual Region Region { get; set; }
        public virtual ICollection<Fertilization> Fertilizations { get; set; }
        public virtual ICollection<Irrigation> Irrigations { get; set; }
        public virtual ICollection<Expense> Expenses { get; set; }
        public virtual ICollection<Sale> Sales { get; set; }
    }
}