using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TarimTakip.API.Models.DTOs.Farm;
using TarimTakip.API.Services;

namespace TarimTakip.API.Controllers
{
    [Route("api/farmfield/{farmFieldId}/fertilization")]
    [ApiController]
    [Authorize(Roles = "Farmer")]
    public class FertilizationController : ControllerBase
    {
        private readonly IFertilizationService _fertilizationService;

        public FertilizationController(IFertilizationService fertilizationService)
        {
            _fertilizationService = fertilizationService;
        }

        // POST: /api/farmfield/2/fertilization
        [HttpPost]
        public async Task<IActionResult> CreateFertilization(int farmFieldId, [FromBody] FertilizationCreateDto request)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                await _fertilizationService.CreateFertilizationAsync(farmFieldId, request, userId);
                return Ok(new { Message = "Gübreleme kaydı başarıyla eklendi." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}