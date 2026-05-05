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
    public class SaleController : ControllerBase
    {
        private readonly ISaleService _saleService;

        public SaleController(ISaleService saleService)
        {
            _saleService = saleService;
        }

        [HttpPost("{farmFieldId}/sale")]
        public async Task<IActionResult> CreateSale(int farmFieldId, [FromBody] SaleCreateDto request)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                await _saleService.CreateSaleAsync(farmFieldId, request, userId);
                return Ok(new { Message = "Satış (gelir) kaydı başarıyla eklendi." });
            }
            catch (Exception ex) { return BadRequest(new { Message = ex.Message }); }
        }

        [HttpGet("{farmFieldId}/sale")]
        public async Task<IActionResult> GetSales(int farmFieldId)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var sales = await _saleService.GetSalesByFieldAsync(farmFieldId, userId);
                return Ok(sales);
            }
            catch (Exception ex) { return BadRequest(new { Message = ex.Message }); }
        }

        [HttpPut("sale/{saleId}")]
        public async Task<IActionResult> UpdateSale(int saleId, [FromBody] SaleCreateDto request)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                await _saleService.UpdateSaleAsync(saleId, request, userId);
                return Ok(new { Message = "Satış kaydı güncellendi." });
            }
            catch (Exception ex) { return BadRequest(new { Message = ex.Message }); }
        }

        [HttpDelete("sale/{saleId}")]
        public async Task<IActionResult> DeleteSale(int saleId)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                await _saleService.DeleteSaleAsync(saleId, userId);
                return Ok(new { Message = "Satış kaydı silindi (Arşivlendi)." });
            }
            catch (Exception ex) { return BadRequest(new { Message = ex.Message }); }
        }
    }
}