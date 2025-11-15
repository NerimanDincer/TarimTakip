using TarimTakip.API.Data.Entities;
using TarimTakip.API.Models.DTOs.Farm;

namespace TarimTakip.API.Services
{
    public interface IFarmFieldService
    {
        Task<FarmField> CreateFarmFieldAsync(FarmFieldCreateDto request, int userId);
        Task<List<FarmFieldListDto>> GetFarmFieldsByUserIdAsync(int userId);
        Task<FarmFieldDetailDto> GetFarmFieldDetailAsync(int farmFieldId, int userId);
        Task<FarmFieldDetailDto> GetFarmFieldDetailForAdminAsync(int farmFieldId);
    }
}