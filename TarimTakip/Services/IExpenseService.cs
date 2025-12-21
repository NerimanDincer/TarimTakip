using TarimTakip.API.Data.Entities;
using TarimTakip.API.Models.DTOs.Farm;

namespace TarimTakip.API.Services
{
    public interface IExpenseService
    {
        Task CreateExpenseAsync(int farmFieldId, ExpenseCreateDto request, int userId);
        Task<List<Expense>> GetExpensesByFieldAsync(int farmFieldId, int userId);
        Task DeleteExpenseAsync(int expenseId, int userId);
    }
}