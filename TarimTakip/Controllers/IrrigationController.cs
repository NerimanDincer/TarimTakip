using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TarimTakip.API.Models.DTOs.Farm;
using TarimTakip.API.Services;

namespace TarimTakip.API.Controllers
{
    [Route("api/farmfield/{farmFieldId}/irrigation")]
    [ApiController]
    [Authorize(Roles = "Farmer")]
    public class IrrigationController : ControllerBase
    {
        private readonly IIrrigationService _irrigationService;

        public IrrigationController(IIrrigationService irrigationService)
        {
            _irrigationService = irrigationService;
        }

        // POST: /api/farmfield/2/irrigation
        [HttpPost]
        public async Task<IActionResult> CreateIrrigation(int farmFieldId, [FromBody] IrrigationCreateDto request)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                await _irrigationService.CreateIrrigationAsync(farmFieldId, request, userId);
                return Ok(new { Message = "Sulama kaydı başarıyla eklendi." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}