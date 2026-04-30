using TarimTakip.Data.Entities;

namespace TarimTakip.API.Data.Entities
{
    public class Sale : BaseEntity
    {
        public int Id { get; set; }
        public int FarmFieldId { get; set; }

        public decimal AmountKg { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }

        public DateTime Date { get; set; }

        public virtual FarmField FarmField { get; set; }
    }

}