using Microsoft.EntityFrameworkCore;
using TarimTakip.API.Data;
using TarimTakip.API.Data.Entities;
using TarimTakip.API.Models.DTOs.Farm;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;


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
        public async Task<IEnumerable<object>> GetIrrigationsByFieldAsync(int farmFieldId, int userId)
        {
            
            var farmField = await _context.FarmFields
                .FirstOrDefaultAsync(f => f.Id == farmFieldId && f.UserId == userId);

            if (farmField == null)
            {
                throw new Exception("Tarla bulunamadı veya bu tarlaya erişim yetkiniz yok.");
            }

            var irrigations = await _context.Irrigations
                .Where(i => i.FarmFieldId == farmFieldId && !i.IsDeleted)
                .OrderByDescending(i => i.Date)
                .Select(i => new
                {
                    Id = i.Id,
                    litersUsed = i.LitersUsed,
                    date = i.Date,
                    description = i.Description
                })
                .ToListAsync();

            return irrigations;
        }
    }
}