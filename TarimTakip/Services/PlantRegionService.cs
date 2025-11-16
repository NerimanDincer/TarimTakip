using Microsoft.EntityFrameworkCore;
using TarimTakip.API.Data;
using TarimTakip.API.Data.Entities;
using TarimTakip.API.Models.DTOs.Plant;

namespace TarimTakip.API.Services
{
    public class PlantRegionService : IPlantRegionService
    {
        private readonly ApplicationDbContext _context;

        public PlantRegionService(ApplicationDbContext context)
        {
            _context = context;
        }

        // SADECE ADMIN: Yeni bölgesel bilgi oluştur
        public async Task<PlantRegionResponseDto> CreatePlantRegionAsync(PlantRegionCreateDto request)
        {
            var exists = await _context.PlantRegions
                .AnyAsync(pr => pr.PlantId == request.PlantId && pr.RegionId == request.RegionId);

            if (exists)
                throw new Exception("Bu bitki için bu bölge bilgisi zaten kayıtlı.");

            var plantRegion = new PlantRegion
            {
                PlantId = request.PlantId,
                RegionId = request.RegionId,
                PlantingTimeStart = request.PlantingTimeStart,
                PlantingTimeEnd = request.PlantingTimeEnd,
                RegionSpecificNotes = request.RegionSpecificNotes
            };

            _context.PlantRegions.Add(plantRegion);
            await _context.SaveChangesAsync();

            // Bu metot, yardımcı metodu kullandığı için zaten DOĞRUYDU.
            return await GetPlantRegionByIdAsync(plantRegion.Id);
        }

        // SADECE ADMIN: Güncelle
        public async Task UpdatePlantRegionAsync(int id, PlantRegionCreateDto request)
        {
            var plantRegion = await _context.PlantRegions.FindAsync(id);
            if (plantRegion == null)
                throw new Exception("Bölgesel bitki kaydı bulunamadı.");

            plantRegion.PlantId = request.PlantId;
            plantRegion.RegionId = request.RegionId;
            plantRegion.PlantingTimeStart = request.PlantingTimeStart;
            plantRegion.PlantingTimeEnd = request.PlantingTimeEnd;
            plantRegion.RegionSpecificNotes = request.RegionSpecificNotes;

            _context.PlantRegions.Update(plantRegion);
            await _context.SaveChangesAsync();
        }

        // SADECE ADMIN: Sil
        public async Task DeletePlantRegionAsync(int id)
        {
            var plantRegion = await _context.PlantRegions.FindAsync(id);
            if (plantRegion == null)
                throw new Exception("Bölgesel bitki kaydı bulunamadı.");

            _context.PlantRegions.Remove(plantRegion);
            await _context.SaveChangesAsync();
        }


        // --- HARİTA İÇİN PUBLIC METOTLAR ---

        // Bir bitkinin tüm bölgelerini getir
        // DÜZELTME BURADA: '.Select(pr => MapToDto(pr))' yerine açık 'Select' ifadesi yazıldı.
        public async Task<List<PlantRegionResponseDto>> GetRegionsForPlantAsync(int plantId)
        {
            return await _context.PlantRegions
                .Where(pr => pr.PlantId == plantId)
                .Include(pr => pr.PlantInfo)
                .Include(pr => pr.Region)
                .Select(pr => new PlantRegionResponseDto
                {
                    Id = pr.Id,
                    PlantId = pr.PlantId,
                    PlantName = pr.PlantInfo.PlantName,
                    RegionId = pr.RegionId,
                    RegionName = pr.Region.Name,
                    PlantingTimeStart = pr.PlantingTimeStart,
                    PlantingTimeEnd = pr.PlantingTimeEnd,
                    RegionSpecificNotes = pr.RegionSpecificNotes
                })
                .ToListAsync();
        }

        // Bir bölgenin tüm bitkilerini getir
        // DÜZELTME BURADA: '.Select(pr => MapToDto(pr))' yerine açık 'Select' ifadesi yazıldı.
        public async Task<List<PlantRegionResponseDto>> GetPlantsForRegionAsync(int regionId)
        {
            return await _context.PlantRegions
                .Where(pr => pr.RegionId == regionId)
                .Include(pr => pr.PlantInfo)
                .Include(pr => pr.Region)
                .Select(pr => new PlantRegionResponseDto
                {
                    Id = pr.Id,
                    PlantId = pr.PlantId,
                    PlantName = pr.PlantInfo.PlantName,
                    RegionId = pr.RegionId,
                    RegionName = pr.Region.Name,
                    PlantingTimeStart = pr.PlantingTimeStart,
                    PlantingTimeEnd = pr.PlantingTimeEnd,
                    RegionSpecificNotes = pr.RegionSpecificNotes
                })
                .ToListAsync();
        }

        // --- YARDIMCI METOTLAR ---

        // Tek bir kaydı DTO olarak getir
        private async Task<PlantRegionResponseDto> GetPlantRegionByIdAsync(int id)
        {
            var plantRegion = await _context.PlantRegions
                .Include(pr => pr.PlantInfo)
                .Include(pr => pr.Region)
                .FirstOrDefaultAsync(pr => pr.Id == id);

            if (plantRegion == null)
                throw new Exception("Bölgesel bitki kaydı bulunamadı.");

            return MapToDto(plantRegion); // Bu metot sadece burada kullanılmalı (SQL'e çevrilmeyen yerde)
        }

        // Entity'yi DTO'ya dönüştür
        private PlantRegionResponseDto MapToDto(PlantRegion pr)
        {
            return new PlantRegionResponseDto
            {
                Id = pr.Id,
                PlantId = pr.PlantId,
                PlantName = pr.PlantInfo.PlantName,
                RegionId = pr.RegionId,
                RegionName = pr.Region.Name,
                PlantingTimeStart = pr.PlantingTimeStart,
                PlantingTimeEnd = pr.PlantingTimeEnd,
                RegionSpecificNotes = pr.RegionSpecificNotes
            };
        }
    }
}