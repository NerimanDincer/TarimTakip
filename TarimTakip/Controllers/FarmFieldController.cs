using Microsoft.AspNetCore.Authorization; // GÜVENLİK İÇİN
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims; // TOKEN'DAN KİMLİK OKUMAK İÇİN
using TarimTakip.API.Models.DTOs.Farm;
using TarimTakip.API.Services;

namespace TarimTakip.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] //(Giriş yapmayan giremez)
    public class FarmFieldController : ControllerBase
    {
        private readonly IFarmFieldService _farmFieldService;

        public FarmFieldController(IFarmFieldService farmFieldService)
        {
            _farmFieldService = farmFieldService;
        }

        // POST: /api/farmfield
        [HttpPost]
        // [Authorize(Roles = "Farmer")] // <-- İŞTE BU SATIR SENİ ENGELLİYORDU, KAPATTIK! 🚫
        public async Task<IActionResult> CreateFarmField([FromBody] FarmFieldCreateDto request)
        {

            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userIdString == null)
            {
                return Unauthorized();
            }

            int userId = int.Parse(userIdString);

            var newFarmField = await _farmFieldService.CreateFarmFieldAsync(request, userId);

            return Ok(newFarmField);
        }
        [HttpGet]
        public async Task<IActionResult> GetMyFarmFields()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userIdString == null)
            {
                return Unauthorized();
            }

            int userId = int.Parse(userIdString);

            var farmFields = await _farmFieldService.GetFarmFieldsByUserIdAsync(userId);

            return Ok(farmFields);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetFarmFieldDetail(int id)
        {
            try
            {
                var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (userIdString == null)
                {
                    return Unauthorized();
                }

                int userId = int.Parse(userIdString);

                var farmFieldDetail = await _farmFieldService
                    .GetFarmFieldDetailAsync(id, userId);

                return Ok(farmFieldDetail);
            }
            catch (Exception ex)
            {
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