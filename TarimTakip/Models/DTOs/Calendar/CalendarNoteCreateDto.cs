using System.ComponentModel.DataAnnotations;

namespace TarimTakip.API.Models.DTOs.Calendar
{
    public class CalendarNoteCreateDto
    {
        [Required]
        public string Title { get; set; }
        public string? Description { get; set; }
        [Required]
        public DateTime Date { get; set; }
    }
}