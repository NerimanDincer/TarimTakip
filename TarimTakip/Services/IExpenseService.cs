using TarimTakip.API.Models.DTOs.Farm;

namespace TarimTakip.API.Services
{
    public interface IExpenseService
    {
        Task CreateExpenseAsync(int farmFieldId, ExpenseCreateDto request, int userId);
    }
}