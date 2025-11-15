using Microsoft.EntityFrameworkCore;
using TarimTakip.API.Data;
using TarimTakip.API.Models.DTOs.Report;

namespace TarimTakip.API.Services
{
    public class ReportService : IReportService
    {
        private readonly ApplicationDbContext _context;

        public ReportService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GeneralReportDto> GetGeneralReportAsync(int userId)
        {
            // 1. Çiftçiye ait TÜM tarlaları, masrafları ve satışlarıyla birlikte çek
            var farmFields = await _context.FarmFields
                .Where(ff => ff.UserId == userId) // Sadece bu çiftçiye ait
                .Include(ff => ff.Expenses) // Tüm masrafları yükle
                .Include(ff => ff.Sales) // Tüm satışları yükle
                .ToListAsync();

            var generalReport = new GeneralReportDto
            {
                FarmFieldReports = new List<FarmFieldReportDto>()
            };

            // 2. Her bir tarla için hesaplama yap
            foreach (var field in farmFields)
            {
                var totalExpenses = field.Expenses.Sum(e => e.Amount);
                var totalIncome = field.Sales.Sum(s => s.Price);
                var netProfit = totalIncome - totalExpenses;

                var fieldReport = new FarmFieldReportDto
                {
                    FarmFieldId = field.Id,
                    PlantName = field.PlantName,
                    TotalExpenses = totalExpenses,
                    TotalIncome = totalIncome,
                    NetProfit = netProfit
                };

                generalReport.FarmFieldReports.Add(fieldReport);
            }

            // 3. Genel toplamları hesapla
            generalReport.OverallTotalExpenses = generalReport.FarmFieldReports.Sum(r => r.TotalExpenses);
            generalReport.OverallTotalIncome = generalReport.FarmFieldReports.Sum(r => r.TotalIncome);
            generalReport.OverallNetProfit = generalReport.FarmFieldReports.Sum(r => r.NetProfit);

            return generalReport;
        }
    }
}