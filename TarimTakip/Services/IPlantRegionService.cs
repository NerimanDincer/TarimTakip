using TarimTakip.API.Models.DTOs.Plant;

namespace TarimTakip.API.Services
{
    public interface IPlantRegionService
    {
        // --- Admin (CRUD) ---
        Task<PlantRegionResponseDto> CreatePlantRegionAsync(PlantRegionCreateDto request);
        Task UpdatePlantRegionAsync(int id, PlantRegionCreateDto request);
        Task DeletePlantRegionAsync(int id);

        // --- Public (Harita Özelliği İçin) ---
        // Bir bitkinin tüm bölgelerini getir (örn: Buğday nerelerde yetişir?)
        Task<List<PlantRegionResponseDto>> GetRegionsForPlantAsync(int plantId);

        // Bir bölgenin tüm bitkilerini getir (örn: Adana'da ne yetişir?)
        Task<List<PlantRegionResponseDto>> GetPlantsForRegionAsync(int regionId);
    }
}