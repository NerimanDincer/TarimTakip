using TarimTakip.API.Models.DTOs.Calendar;

namespace TarimTakip.API.Services
{
    public interface ICalendarNoteService
    {
        Task<List<CalendarNoteResponseDto>> GetNotesAsync(int userId, DateTime startDate, DateTime endDate);
        Task<CalendarNoteResponseDto> CreateNoteAsync(CalendarNoteCreateDto request, int userId);
        Task UpdateNoteAsync(int noteId, CalendarNoteCreateDto request, int userId);
        Task DeleteNoteAsync(int noteId, int userId);
    }
}