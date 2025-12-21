using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TarimTakip.API.Models.DTOs.Report; // DTO'yu kullanmak için ekledik
using TarimTakip.API.Services;

namespace TarimTakip.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // DİKKAT: Burayı genel yaptık. Mühendisler de girebilsin.
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        // 1. FİNANSAL RAPOR (Eski Metot)
        // Buraya özel kısıtlama koyduk: Sadece Çiftçiler finansal raporunu görebilir.
        [HttpGet("general")]
        [Authorize(Roles = "Farmer")]
        public async Task<IActionResult> GetGeneralReport()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var report = await _reportService.GetGeneralReportAsync(userId);
            return Ok(report);
        }

        // 2. İLAN ŞİKAYET ETME (Yeni Metot)
        // Herkes (Çiftçi + Mühendis) kullanabilir.
        [HttpPost("create")]
        public async Task<IActionResult> ReportAdvert([FromBody] ReportCreateDto request)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                await _reportService.CreateReportAsync(request, userId);
                return Ok(new { Message = "Şikayetiniz başarıyla alındı." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}