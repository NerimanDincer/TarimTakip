using TarimTakip.API.Models.DTOs.Farm;

namespace TarimTakip.API.Services
{
    public interface IIrrigationService
    {
        Task CreateIrrigationAsync(int farmFieldId, IrrigationCreateDto request, int userId);
    }
}