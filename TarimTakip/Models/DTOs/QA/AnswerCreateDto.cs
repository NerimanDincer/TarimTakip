using System.ComponentModel.DataAnnotations;

namespace TarimTakip.API.Models.DTOs.QA
{
    public class AnswerCreateDto
    {
        [Required]
        public string Content { get; set; } //mühendisin cevabı
    }
}