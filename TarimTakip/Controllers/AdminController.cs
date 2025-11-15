using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TarimTakip.API.Services;

namespace TarimTakip.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")] // <-- SİHİRLİ SATIR! SADECE ADMİNLER
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        // GET: /api/Admin/users
        // Tüm kullanıcıları listeler
        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _adminService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpDelete("users/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                await _adminService.DeleteUserAsync(id);
                return Ok(new { Message = "Kullanıcı başarıyla silindi." });
            }
            catch (Exception ex)
            {
                // "Admin silinemez" veya "Kullanıcı bulunamadı" hatalarını yakala
                // Veya veritabanı kısıtlaması (FOREIGN KEY) hatasını yakala
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
