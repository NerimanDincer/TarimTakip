using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TarimTakip.API.Models.DTOs.Farm;
using TarimTakip.API.Services;

namespace TarimTakip.API.Controllers
{
    // API YOLU (ROUTE) ÇOK ÖNEMLİ:
    // Bu, "Tarlaların altındaki masraflar" anlamına gelir
    [Route("api/farmfield/{farmFieldId}/expense")]
    [ApiController]
    [Authorize(Roles = "Farmer")] // Sadece Çiftçiler
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseService _expenseService;

        public ExpenseController(IExpenseService expenseService)
        {
            _expenseService = expenseService;
        }

        // POST: /api/farmfield/5/expense  (5 numaralı tarlaya masraf ekle)
        [HttpPost]
        public async Task<IActionResult> CreateExpense(int farmFieldId, [FromBody] ExpenseCreateDto request)
        {
            try
            {
                // Token'dan (kimlik kartından) çiftçinin ID'sini al
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

                // Servise hem URL'den gelen 'farmFieldId'yi hem de token'dan gelen 'userId'yi gönder
                await _expenseService.CreateExpenseAsync(farmFieldId, request, userId);

                return Ok(new { Message = "Masraf başarıyla eklendi." });
            }
            catch (Exception ex)
            {
                // Servis'te attığımız "Yetkiniz yok" veya "Bulunamadı" hatalarını yakala
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}