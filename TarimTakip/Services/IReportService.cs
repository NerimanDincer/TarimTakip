using TarimTakip.API.Models.DTOs.Report;

namespace TarimTakip.API.Services
{
    public interface IReportService
    {
        Task<GeneralReportDto> GetGeneralReportAsync(int userId);
    }
}