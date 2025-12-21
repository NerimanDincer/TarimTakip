using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TarimTakip.API.Services;
using TarimTakip.API.Models.DTOs.Admin;

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
        // GET: /api/Admin/stats
        // Admin paneli için ana istatistikleri getirir
        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            var stats = await _adminService.GetAdminStatsAsync();
            return Ok(stats);
        }
        // PUT: /api/Admin/users/toggle-status/2
        // Bir kullanıcının (ID: 2) 'IsActive' durumunu tersine çevirir
        [HttpPut("users/toggle-status/{id}")]
        public async Task<IActionResult> ToggleUserStatus(int id)
        {
            try
            {
                var newStatus = await _adminService.ToggleUserStatusAsync(id);
                var statusMessage = newStatus ? "aktif" : "pasif";

                return Ok(new
                {
                    Message = $"Kullanıcı başarıyla {statusMessage} hale getirildi.",
                    NewStatus = newStatus
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
        // PUT: /api/Admin/users/role/4
        // Bir kullanıcının (ID: 4) rolünü günceller
        [HttpPut("users/role/{id}")]
        public async Task<IActionResult> UpdateUserRole(int id, [FromBody] AdminUserRoleUpdateDto request)
        {
            try
            {
                await _adminService.UpdateUserRoleAsync(id, request.NewRole);
                return Ok(new { Message = $"Kullanıcı rolü başarıyla '{request.NewRole}' olarak güncellendi." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // Şikayetleri Gör
        [HttpGet("reports")]
        public async Task<IActionResult> GetReports()
        {
            var reports = await _adminService.GetAllReportsAsync();
            return Ok(reports);
        }

        // İlanı Sil (Yargı Dağıt) 🔨
        [HttpDelete("advert/{id}")]
        public async Task<IActionResult> DeleteAdvert(int id)
        {
            try
            {
                await _adminService.DeleteAdvertAsync(id);
                return Ok(new { Message = "İlan başarıyla silindi." });
            }
            catch (Exception ex)
            {
                // Hata mesajını 404 Not Found olarak döndür
                return NotFound(new { Message = ex.Message });
            }
        }

        // Şikayeti Sil (İlan masumsa şikayeti çöpe at)
        [HttpDelete("report/{id}")]
        public async Task<IActionResult> DismissReport(int id)
        {
            try
            {
                await _adminService.DeleteReportAsync(id);
                return Ok(new { Message = "Şikayet kaydı başarıyla silindi." });
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }
    }
}
