using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TarimTakip.API.Models.DTOs.Farm;
using TarimTakip.API.Services;

namespace TarimTakip.API.Controllers
{
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

        // GET: /api/farmfield/5/expense (Listele)
        [HttpGet]
        public async Task<IActionResult> GetExpenses(int farmFieldId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var expenses = await _expenseService.GetExpensesByFieldAsync(farmFieldId, userId);
            return Ok(expenses);
        }

        // DELETE: /api/farmfield/expense/15 (Sil) -> Dikkat Route değişti biraz
        [HttpDelete("/api/expense/{expenseId}")]
        public async Task<IActionResult> DeleteExpense(int expenseId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            try
            {
                await _expenseService.DeleteExpenseAsync(expenseId, userId);
                return Ok(new { Message = "Masraf silindi (Arşivlendi)." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}