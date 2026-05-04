using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims; // ID'yi token'dan okumak için
using TarimTakip.API.Models.DTOs.Auth;
using TarimTakip.API.Services;

namespace TarimTakip.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Sadece giriş yapmışlar profilini görebilir
    public class UserController : ControllerBase
    {
        private readonly IAuthService _authService;

        public UserController(IAuthService authService)
        {
            _authService = authService;
        }

        // GET: /api/User/profile
        // Kendi profilimi getir
        [HttpGet("profile")]
        public async Task<IActionResult> GetMyProfile()
        {
            // Token'ın içinden ID'yi okuma sihri:
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userId = int.Parse(userIdString);

            var user = await _authService.GetUserByIdAsync(userId);

            // Şifreyi (hash) gizleyip dönelim
            return Ok(new
            {
                user.Id,
                user.FullName,
                user.Email,
                user.Phone,
                user.Bio,
                user.ProfilePictureUrl,
                Region = user.Region?.Name
            });
        }

        // PUT: /api/User/profile
        // Profilimi güncelle
        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UserUpdateDto request)
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userId = int.Parse(userIdString);

            try
            {
                await _authService.UpdateProfileAsync(userId, request);
                return Ok(new { Message = "Profil başarıyla güncellendi." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}