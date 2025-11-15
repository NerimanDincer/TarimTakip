using System.ComponentModel.DataAnnotations;

namespace TarimTakip.API.Models.DTOs.Farm
{
    public class ExpenseCreateDto
    {
        [Required]
        public string CostType { get; set; } // Gübre, Mazot, İlaç vb.

        [Required]
        public decimal Amount { get; set; } // Tutar

        [Required]
        public DateTime Date { get; set; }

        public string? Note { get; set; }
    }
} //(Not: Hangi tarlaya ekleneceğini URL'den alacağız, DTO'ya koymaya gerek yok.)
