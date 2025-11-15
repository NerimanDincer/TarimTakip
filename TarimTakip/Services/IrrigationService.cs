using TarimTakip.API.Data;
using TarimTakip.API.Data.Entities;
using TarimTakip.API.Models.DTOs.Farm;

namespace TarimTakip.API.Services
{
    public class IrrigationService : IIrrigationService
    {
        private readonly ApplicationDbContext _context;

        public IrrigationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateIrrigationAsync(int farmFieldId, IrrigationCreateDto request, int userId)
        {
            // 1. GÜVENLİK KONTROLÜ
            var farmField = await _context.FarmFields.FindAsync(farmFieldId);

            if (farmField == null)
            {
                throw new Exception("Tarla bulunamadı.");
            }

            if (farmField.UserId != userId)
            {
                throw new Exception("Bu tarlaya sulama kaydı ekleme yetkiniz yok.");
            }

            // 2. KAYIT OLUŞTURMA
            var irrigation = new Irrigation
            {
                FarmFieldId = farmFieldId,
                LitersUsed = request.LitersUsed,
                Description = request.Description,
                Date = request.Date
            };

            await _context.Irrigations.AddAsync(irrigation);
            await _context.SaveChangesAsync();
        }
    }
}