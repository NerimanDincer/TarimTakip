using TarimTakip.API.Models.DTOs.Plant;

namespace TarimTakip.API.Services
{
    public interface IPlantInfoService
    {
        Task<List<PlantInfoResponseDto>> GetAllPlantsAsync();
        Task<PlantInfoResponseDto> GetPlantByIdAsync(int id);
        Task<PlantInfoResponseDto> CreatePlantAsync(PlantInfoCreateDto request);
        Task UpdatePlantAsync(int id, PlantInfoCreateDto request);
        Task DeletePlantAsync(int id);
    }
}