using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TarimTakip.API.Services;

namespace TarimTakip.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Farmer")] // Sadece Çiftçiler
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        // GET: /api/report/general
        [HttpGet("general")]
        public async Task<IActionResult> GetGeneralReport()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var report = await _reportService.GetGeneralReportAsync(userId);
            return Ok(report);
        }
    }
}