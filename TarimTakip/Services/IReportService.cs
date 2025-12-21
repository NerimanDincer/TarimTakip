using TarimTakip.API.Models.DTOs.Report;

namespace TarimTakip.API.Services
{
    public interface IReportService
    {
        Task<GeneralReportDto> GetGeneralReportAsync(int userId);
        Task CreateReportAsync(ReportCreateDto request, int reporterId);
    }
}