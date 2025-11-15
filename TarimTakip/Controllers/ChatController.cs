using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TarimTakip.API.Services;

namespace TarimTakip.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Sadece giriş yapanlar
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        // GET: /api/Chat/rooms
        // Giriş yapan kullanıcının tüm sohbet odalarını listeler
        [HttpGet("rooms")]
        public async Task<IActionResult> GetMyRooms()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var rooms = await _chatService.GetChatRoomsForUserAsync(userId);
            return Ok(rooms);
        }

        // POST: /api/Chat/start/{engineerId}
        // Bir mühendisle sohbet başlatır (Sadece Çiftçiler)
        [HttpPost("start/{engineerId}")]
        [Authorize(Roles = "Farmer")]
        public async Task<IActionResult> StartChat(int engineerId)
        {
            try
            {
                var farmerId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var room = await _chatService.FindOrCreateChatRoomAsync(farmerId, engineerId);
                return Ok(room); // Odanın detaylarını (ve eski mesajları) döndür
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}