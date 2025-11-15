using TarimTakip.API.Data;
using TarimTakip.API.Data.Entities;
using TarimTakip.API.Models.DTOs.Farm;
using Microsoft.EntityFrameworkCore; // Bu satırın eklendiğinden emin ol

namespace TarimTakip.API.Services
{
    public class FarmFieldService : IFarmFieldService
    {
        private readonly ApplicationDbContext _context;

        public FarmFieldService(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. METOT (Mevcut metot - Tarla Oluşturma)
        public async Task<FarmField> CreateFarmFieldAsync(FarmFieldCreateDto request, int userId)
        {
            // DTO'dan gelen veriyi veritabanı Entity'sine dönüştür
            var farmField = new FarmField
            {
                UserId = userId,
                PlantName = request.PlantName,
                SowingDate = request.SowingDate,
                Area = request.Area,
                SoilInfo = request.SoilInfo,
                RegionId = request.RegionId,
            };

            await _context.FarmFields.AddAsync(farmField);
            await _context.SaveChangesAsync();
            return farmField;
        }

        // 2. METOT (YENİ - Tarlaları Listeleme)
        public async Task<List<FarmFieldListDto>> GetFarmFieldsByUserIdAsync(int userId)
        {
            return await _context.FarmFields
                .Where(ff => ff.UserId == userId) // Sadece bu kullanıcıya ait olanlar
                .Include(ff => ff.Region) // Region tablosuna JOIN at (Bölge adını almak için)
                .Select(ff => new FarmFieldListDto // DTO'ya dönüştür
                {
                    Id = ff.Id,
                    PlantName = ff.PlantName,
                    Area = ff.Area,
                    RegionName = ff.Region.Name
                })
                .ToListAsync();
        }

        // 3. METOT (YENİ - Tarla Detayını Getirme)
        public async Task<FarmFieldDetailDto> GetFarmFieldDetailAsync(int farmFieldId, int userId)
        {
            // Tarlayı, tüm alt kayıtlarıyla (Masraf, Sulama vb.) birlikte çek
            // GÜVENLİK KONTROLÜ: Sorgu (Where) kısmı hem tarla ID'sini hem de kullanıcı ID'sini kontrol eder
            var farmField = await _context.FarmFields
                .Where(ff => ff.Id == farmFieldId && ff.UserId == userId) // <-- GÜVENLİK BURADA!
                .Include(ff => ff.Region) // Bölge adı için
                .Include(ff => ff.Expenses) // Masraflar listesi için
                .Include(ff => ff.Irrigations) // Sulamalar listesi için
                .Include(ff => ff.Fertilizations) // Gübrelemeler listesi için
                .Select(ff => new FarmFieldDetailDto // DTO'ya dönüştür
                {
                    Id = ff.Id,
                    PlantName = ff.PlantName,
                    SowingDate = ff.SowingDate,
                    Area = ff.Area,
                    SoilInfo = ff.SoilInfo,
                    RegionName = ff.Region.Name,
                    // Alt listeleri de DTO'lara dönüştür
                    Expenses = ff.Expenses.Select(e => new ExpenseResponseDto
                    {
                        Id = e.Id,
                        CostType = e.CostType,
                        Amount = e.Amount,
                        Date = e.Date,
                        Note = e.Note
                    }).ToList(),
                    Irrigations = ff.Irrigations.Select(i => new IrrigationResponseDto
                    {
                        Id = i.Id,
                        LitersUsed = i.LitersUsed,
                        Description = i.Description,
                        Date = i.Date
                    }).ToList(),
                    Fertilizations = ff.Fertilizations.Select(f => new FertilizationResponseDto
                    {
                        Id = f.Id,
                        Description = f.Description,
                        Date = f.Date
                    }).ToList()
                })
                .FirstOrDefaultAsync(); // Tek bir kayıt veya null

            // Tarla ya bulunamadı, ya da başka birine ait
            if (farmField == null)
            {
                throw new Exception("Tarla bulunamadı veya bu tarlayı görme yetkiniz yok.");
            }

            return farmField;
        }
        public async Task<FarmFieldDetailDto> GetFarmFieldDetailForAdminAsync(int farmFieldId)
        {

            var farmField = await _context.FarmFields
                .Where(ff => ff.Id == farmFieldId)
                .Include(ff => ff.Region) // Bölge adı için
                .Include(ff => ff.Expenses) // Masraflar listesi için
                .Include(ff => ff.Irrigations) // Sulamalar listesi için
                .Include(ff => ff.Fertilizations) // Gübrelemeler listesi için
                .Select(ff => new FarmFieldDetailDto // DTO'ya dönüştür
                {
                    Id = ff.Id,
                    PlantName = ff.PlantName,
                    SowingDate = ff.SowingDate,
                    Area = ff.Area,
                    SoilInfo = ff.SoilInfo,
                    RegionName = ff.Region.Name,
                    // Alt listeleri de DTO'lara dönüştür
                    Expenses = ff.Expenses.Select(e => new ExpenseResponseDto
                    {
                        Id = e.Id,
                        CostType = e.CostType,
                        Amount = e.Amount,
                        Date = e.Date,
                        Note = e.Note
                    }).ToList(),
                    Irrigations = ff.Irrigations.Select(i => new IrrigationResponseDto
                    {
                        Id = i.Id,
                        LitersUsed = i.LitersUsed,
                        Description = i.Description,
                        Date = i.Date
                    }).ToList(),
                    Fertilizations = ff.Fertilizations.Select(f => new FertilizationResponseDto
                    {
                        Id = f.Id,
                        Description = f.Description,
                        Date = f.Date
                    }).ToList()
                })
                .FirstOrDefaultAsync(); // Tek bir kayıt veya null

            // Tarla ya bulunamadı, ya da başka birine ait
            if (farmField == null)
            {
                throw new Exception("Tarla bulunamadı.");
            }

            return farmField;
        }
    }
}