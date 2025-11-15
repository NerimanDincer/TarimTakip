using Microsoft.EntityFrameworkCore;
using TarimTakip.API.Data;
using TarimTakip.API.Data.Entities;
using TarimTakip.API.Models.DTOs.Calendar;

namespace TarimTakip.API.Services
{
    public class CalendarNoteService : ICalendarNoteService
    {
        private readonly ApplicationDbContext _context;

        public CalendarNoteService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<CalendarNoteResponseDto>> GetNotesAsync(int userId, DateTime startDate, DateTime endDate)
        {
            // Kullanıcının notlarını, iki tarih arasında getir
            return await _context.CalendarNotes
                .Where(n => n.UserId == userId && n.Date >= startDate && n.Date <= endDate)
                .Select(n => new CalendarNoteResponseDto
                {
                    Id = n.Id,
                    Title = n.Title,
                    Description = n.Description,
                    Date = n.Date
                })
                .OrderBy(n => n.Date)
                .ToListAsync();
        }

        public async Task<CalendarNoteResponseDto> CreateNoteAsync(CalendarNoteCreateDto request, int userId)
        {
            var note = new CalendarNote
            {
                Title = request.Title,
                Description = request.Description,
                Date = request.Date,
                UserId = userId // Token'dan gelen kullanıcıya bağla
            };

            await _context.CalendarNotes.AddAsync(note);
            await _context.SaveChangesAsync();

            // Oluşturulan notu DTO olarak geri döndür
            return new CalendarNoteResponseDto
            {
                Id = note.Id,
                Title = note.Title,
                Description = note.Description,
                Date = note.Date
            };
        }

        public async Task UpdateNoteAsync(int noteId, CalendarNoteCreateDto request, int userId)
        {
            // 1. Notu bul
            var note = await _context.CalendarNotes.FindAsync(noteId);

            // 2. Güvenlik Kontrolü: Not bulundu mu VE bu kullanıcıya mı ait?
            if (note == null || note.UserId != userId)
            {
                throw new Exception("Not bulunamadı veya bu notu güncelleme yetkiniz yok.");
            }

            // 3. Güncelle
            note.Title = request.Title;
            note.Description = request.Description;
            note.Date = request.Date;

            _context.CalendarNotes.Update(note);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteNoteAsync(int noteId, int userId)
        {
            // 1. Notu bul
            var note = await _context.CalendarNotes.FindAsync(noteId);

            // 2. Güvenlik Kontrolü: Not bulundu mu VE bu kullanıcıya mı ait?
            if (note == null || note.UserId != userId)
            {
                throw new Exception("Not bulunamadı veya bu notu silme yetkiniz yok.");
            }

            // 3. Sil
            _context.CalendarNotes.Remove(note);
            await _context.SaveChangesAsync();
        }
    }
}