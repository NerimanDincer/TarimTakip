namespace TarimTakip.API.Data.Entities
{
    public class Expense
    {
        public int Id { get; set; }
        public int FarmFieldId { get; set; } // Foreign Key
        public string CostType { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string? Note { get; set; }

        // Navigation Property
        public virtual FarmField FarmField { get; set; }
    }
}