using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TarimTakip.API.Models.DTOs.Farm;
using TarimTakip.API.Services;

namespace TarimTakip.API.Controllers
{
    [Route("api/farmfield")]
    [ApiController]
    [Authorize(Roles = "Farmer")]
    public class FertilizationController : ControllerBase
    {
        private readonly IFertilizationService _fertilizationService;

        public FertilizationController(IFertilizationService fertilizationService)
        {
            _fertilizationService = fertilizationService;
        }

        [HttpPost("{farmFieldId}/fertilization")]
        public async Task<IActionResult> CreateFertilization(int farmFieldId, [FromBody] FertilizationCreateDto request)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                await _fertilizationService.CreateFertilizationAsync(farmFieldId, request, userId);
                return Ok(new { Message = "Gübreleme kaydı başarıyla eklendi." });
            }
            catch (Exception ex) { return BadRequest(new { Message = ex.Message }); }
        }

        [HttpGet("{farmFieldId}/fertilization")]
        public async Task<IActionResult> GetFertilizations(int farmFieldId)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var list = await _fertilizationService.GetFertilizationsByFieldAsync(farmFieldId, userId);
                return Ok(list);
            }
            catch (Exception ex) { return BadRequest(new { Message = ex.Message }); }
        }

        [HttpPut("fertilization/{id}")]
        public async Task<IActionResult> UpdateFertilization(int id, [FromBody] FertilizationCreateDto request)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                await _fertilizationService.UpdateFertilizationAsync(id, request, userId);
                return Ok(new { Message = "Gübreleme kaydı güncellendi." });
            }
            catch (Exception ex) { return BadRequest(new { Message = ex.Message }); }
        }

        [HttpDelete("fertilization/{id}")]
        public async Task<IActionResult> DeleteFertilization(int id)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                await _fertilizationService.DeleteFertilizationAsync(id, userId);
                return Ok(new { Message = "Gübreleme kaydı silindi (Arşivlendi)." });
            }
            catch (Exception ex) { return BadRequest(new { Message = ex.Message }); }
        }
    }
}