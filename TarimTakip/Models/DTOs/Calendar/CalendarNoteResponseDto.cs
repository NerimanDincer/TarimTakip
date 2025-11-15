namespace TarimTakip.API.Models.DTOs.Calendar
{
    public class CalendarNoteResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime Date { get; set; }
    }
}