using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TarimTakip.API.Models.DTOs.Calendar;
using TarimTakip.API.Services;

namespace TarimTakip.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Hem Çiftçi hem Mühendis kullanabilir
    public class CalendarNoteController : ControllerBase
    {
        private readonly ICalendarNoteService _calendarNoteService;

        public CalendarNoteController(ICalendarNoteService calendarNoteService)
        {
            _calendarNoteService = calendarNoteService;
        }

        // POST: /api/CalendarNote
        [HttpPost]
        public async Task<IActionResult> CreateNote([FromBody] CalendarNoteCreateDto request)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var newNote = await _calendarNoteService.CreateNoteAsync(request, userId);
            return Ok(newNote);
        }

        // GET: /api/CalendarNote?startDate=2025-11-01&endDate=2025-11-30
        [HttpGet]
        public async Task<IActionResult> GetNotes([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var notes = await _calendarNoteService.GetNotesAsync(userId, startDate, endDate);
            return Ok(notes);
        }

        // PUT: /api/CalendarNote/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNote(int id, [FromBody] CalendarNoteCreateDto request)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                await _calendarNoteService.UpdateNoteAsync(id, request, userId);
                return Ok(new { Message = "Not başarıyla güncellendi." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // DELETE: /api/CalendarNote/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNote(int id)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                await _calendarNoteService.DeleteNoteAsync(id, userId);
                return Ok(new { Message = "Not başarıyla silindi." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}