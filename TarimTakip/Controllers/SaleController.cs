using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TarimTakip.API.Models.DTOs.Farm;
using TarimTakip.API.Services;

namespace TarimTakip.API.Controllers
{
    [Route("api/farmfield/{farmFieldId}/sale")]
    [ApiController]
    [Authorize(Roles = "Farmer")]
    public class SaleController : ControllerBase
    {
        private readonly ISaleService _saleService;

        public SaleController(ISaleService saleService)
        {
            _saleService = saleService;
        }

        // POST: /api/farmfield/2/sale
        [HttpPost]
        public async Task<IActionResult> CreateSale(int farmFieldId, [FromBody] SaleCreateDto request)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                await _saleService.CreateSaleAsync(farmFieldId, request, userId);
                return Ok(new { Message = "Satış (gelir) kaydı başarıyla eklendi." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}