using Microsoft.EntityFrameworkCore;
using TarimTakip.API.Data;
using TarimTakip.API.Data.Entities; // Report entity'si için gerekli
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

        // --- 1. Bölüm: Finansal Raporlar (Eski Kodların) ---
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
                var totalIncome = field.Sales.Sum(s => s.TotalPrice);
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

        // --- 2. Bölüm: İlan Şikayet Etme (YENİ EKLENEN KISIM) ---
        public async Task CreateReportAsync(ReportCreateDto request, int reporterId)
        {
            // 1. İlan gerçekten var mı?
            var advert = await _context.Adverts.FindAsync(request.AdvertId);
            if (advert == null)
                throw new Exception("Şikayet edilen ilan bulunamadı.");

            // 2. Kişi kendi ilanını şikayet edemez
            if (advert.SellerId == reporterId)
                throw new Exception("Kendi ilanınızı şikayet edemezsiniz.");

            // 3. Şikayeti oluştur
            // (Tam yol belirttik ki DTO ile Entity isimleri karışmasın)
            var report = new TarimTakip.API.Data.Entities.Report
            {
                AdvertId = request.AdvertId,
                ReporterId = reporterId,
                Reason = request.Reason,
                CreatedAt = DateTime.UtcNow
            };

            await _context.Reports.AddAsync(report);
            await _context.SaveChangesAsync();
        }
    }
}