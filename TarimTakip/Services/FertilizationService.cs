using TarimTakip.API.Data;
using TarimTakip.API.Data.Entities;
using TarimTakip.API.Models.DTOs.Farm;

namespace TarimTakip.API.Services
{
    public class FertilizationService : IFertilizationService
    {
        private readonly ApplicationDbContext _context;

        public FertilizationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateFertilizationAsync(int farmFieldId, FertilizationCreateDto request, int userId)
        {
            // 1. GÜVENLİK KONTROLÜ
            var farmField = await _context.FarmFields.FindAsync(farmFieldId);

            if (farmField == null)
            {
                throw new Exception("Tarla bulunamadı.");
            }

            if (farmField.UserId != userId)
            {
                throw new Exception("Bu tarlaya gübreleme kaydı ekleme yetkiniz yok.");
            }

            // 2. KAYIT OLUŞTURMA
            var fertilization = new Fertilization
            {
                FarmFieldId = farmFieldId,
                Description = request.Description,
                Date = request.Date
            };

            await _context.Fertilizations.AddAsync(fertilization);
            await _context.SaveChangesAsync();
        }
    }
}