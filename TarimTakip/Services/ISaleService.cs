using TarimTakip.API.Models.DTOs.Farm;

namespace TarimTakip.API.Services
{
    public interface ISaleService
    {
        Task CreateSaleAsync(int farmFieldId, SaleCreateDto request, int userId);
    }
}