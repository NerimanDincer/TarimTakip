using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TarimTakip.API.Models.DTOs.Advert;
using TarimTakip.API.Services;

namespace TarimTakip.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdvertController : ControllerBase
    {
        private readonly IAdvertService _advertService;

        public AdvertController(IAdvertService advertService)
        {
            _advertService = advertService;
        }

        // GET: /api/Advert (Herkes görebilir mi? Şimdilik giriş yapanlar görsün)
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var adverts = await _advertService.GetAllAdvertsAsync();
            return Ok(adverts);
        }

        // POST: /api/Advert (Sadece Çiftçiler satabilsin mi? Yoksa herkes mi?)
        // Şimdilik herkes satsın, ticaret canlansın :)
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateAdvert([FromBody] AdvertCreateDto request)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                await _advertService.CreateAdvertAsync(request, userId);
                return Ok(new { Message = "İlan başarıyla oluşturuldu." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}