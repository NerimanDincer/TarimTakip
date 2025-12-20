using System.ComponentModel.DataAnnotations;

namespace TarimTakip.API.Models.DTOs.QA
{
    public class QuestionCreateDto
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        public string? ImageUrl { get; set; }
    }
}