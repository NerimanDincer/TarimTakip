using System.Collections.Generic;
using System.Threading.Tasks;

using TarimTakip.API.Models.DTOs.Farm;

namespace TarimTakip.API.Services
{
    public interface IIrrigationService
    {
        Task CreateIrrigationAsync(int farmFieldId, IrrigationCreateDto request, int userId);
        Task<IEnumerable<object>> GetIrrigationsByFieldAsync(int farmFieldId, int userId);
    }
}