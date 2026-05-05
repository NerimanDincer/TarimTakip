using TarimTakip.API.Models.DTOs.Farm;

namespace TarimTakip.API.Services
{
    public interface IFertilizationService
    {
        Task CreateFertilizationAsync(int farmFieldId, FertilizationCreateDto request, int userId);
        Task<List<object>> GetFertilizationsByFieldAsync(int farmFieldId, int userId);
        Task UpdateFertilizationAsync(int fertId, FertilizationCreateDto request, int userId);
        Task DeleteFertilizationAsync(int fertId, int userId);
    }
}