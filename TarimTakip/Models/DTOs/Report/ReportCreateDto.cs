using System.ComponentModel.DataAnnotations;

namespace TarimTakip.API.Models.DTOs.Report
{
    public class ReportCreateDto
    {
        [Required]
        public int AdvertId { get; set; } // Hangi ilanı şikayet ediyoruz?

        [Required(ErrorMessage = "Şikayet sebebini yazmalısınız.")]
        [MaxLength(500)] // Destan yazmasınlar :)
        public string Reason { get; set; }
    }
}