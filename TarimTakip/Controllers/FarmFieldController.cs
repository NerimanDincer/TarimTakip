using Microsoft.AspNetCore.Authorization; // GÜVENLİK İÇİN
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims; // TOKEN'DAN KİMLİK OKUMAK İÇİN
using TarimTakip.API.Models.DTOs.Farm;
using TarimTakip.API.Services;

namespace TarimTakip.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // <-- SİHİRLİ SATIR 1: BU KAPIDAN GİRİŞ YASAK! (Giriş yapmayan giremez)
    public class FarmFieldController : ControllerBase
    {
        private readonly IFarmFieldService _farmFieldService;

        public FarmFieldController(IFarmFieldService farmFieldService)
        {
            _farmFieldService = farmFieldService;
        }

        // POST: /api/farmfield
        [HttpPost]
        [Authorize(Roles = "Farmer")] // <-- SİHİRLİ SATIR 2: SADECE ÇİFTÇİLER GİREBİLİR!
        public async Task<IActionResult> CreateFarmField([FromBody] FarmFieldCreateDto request)
        {

            // Token (kimlik kartı) içinden kullanıcının ID'sini (Claim) oku
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                // Bu normalde [Authorize] olduğu için hiç olmaz ama ek güvenlik
                return Unauthorized();
            }

            var userId = int.Parse(userIdClaim.Value);

            // Servise hem DTO'yu hem de token'dan aldığımız userId'yi gönder
            var newFarmField = await _farmFieldService.CreateFarmFieldAsync(request, userId);
            return Ok(newFarmField); // Oluşturulan tarlanın tüm bilgilerini (ID dahil) döndür
        }
        [HttpGet]
        public async Task<IActionResult> GetMyFarmFields()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var farmFields = await _farmFieldService.GetFarmFieldsByUserIdAsync(userId);
            return Ok(farmFields);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFarmFieldDetail(int id)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

                // Servisteki metot ZATEN güvenlik kontrolü yapıyor (UserId'yi yolladık)
                var farmFieldDetail = await _farmFieldService.GetFarmFieldDetailAsync(id, userId);

                return Ok(farmFieldDetail);
            }
            catch (Exception ex)
            {
                // Serviste attığımız "Tarla bulunamadı..." hatasını yakala
                return NotFound(new { Message = ex.Message });
            }
        }
        [HttpGet("admin/{id}")]
        [Authorize(Roles = "Admin")] // <-- SİHİRLİ KİLİT!
        public async Task<IActionResult> GetFarmFieldDetailForAdmin(int id)
        {
            try
            {
                // Admin'e özel olan, güvenlik kontrolü yapmayan servisi çağır
                var farmFieldDetail = await _farmFieldService.GetFarmFieldDetailForAdminAsync(id);
                return Ok(farmFieldDetail);
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }
    }
}