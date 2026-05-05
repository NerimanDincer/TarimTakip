using Microsoft.EntityFrameworkCore;
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
            var field = await _context.FarmFields.FirstOrDefaultAsync(f => f.Id == farmFieldId && f.UserId == userId);
            if (field == null) throw new Exception("Tarla bulunamadı.");

            var fert = new Fertilization
            {
                FarmFieldId = farmFieldId,
                Description = request.Description,
                Date = request.Date,
                IsDeleted = false,
                CreatedDate = DateTime.UtcNow
            };

            await _context.Fertilizations.AddAsync(fert);
            await _context.SaveChangesAsync();
        }

        public async Task<List<object>> GetFertilizationsByFieldAsync(int farmFieldId, int userId)
        {
            var field = await _context.FarmFields.FirstOrDefaultAsync(f => f.Id == farmFieldId && f.UserId == userId);
            if (field == null) throw new Exception("Tarla bulunamadı.");

            return await _context.Fertilizations
                .Where(f => f.FarmFieldId == farmFieldId && !f.IsDeleted)
                .OrderByDescending(f => f.Date)
                .Select(f => new { f.Id, f.Description, f.Date })
                .ToListAsync<object>();
        }

        public async Task UpdateFertilizationAsync(int fertId, FertilizationCreateDto request, int userId)
        {
            var fert = await _context.Fertilizations.Include(f => f.FarmField)
                .FirstOrDefaultAsync(f => f.Id == fertId && f.FarmField.UserId == userId && !f.IsDeleted);

            if (fert == null) throw new Exception("Kayıt bulunamadı.");

            fert.Description = request.Description;
            fert.Date = request.Date;

            _context.Fertilizations.Update(fert);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteFertilizationAsync(int fertId, int userId)
        {
            var fert = await _context.Fertilizations.Include(f => f.FarmField)
                .FirstOrDefaultAsync(f => f.Id == fertId && f.FarmField.UserId == userId);

            if (fert == null) throw new Exception("Kayıt bulunamadı.");

            fert.IsDeleted = true; // Soft Delete
            _context.Fertilizations.Update(fert);
            await _context.SaveChangesAsync();
        }
    }
}