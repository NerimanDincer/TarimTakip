using TarimTakip.API.Models.DTOs.Farm;

namespace TarimTakip.API.Services
{
    public interface ISaleService
    {
        Task CreateSaleAsync(int farmFieldId, SaleCreateDto request, int userId);
        Task<List<object>> GetSalesByFieldAsync(int farmFieldId, int userId);
        Task UpdateSaleAsync(int saleId, SaleCreateDto request, int userId);
        Task DeleteSaleAsync(int saleId, int userId);
    }
}