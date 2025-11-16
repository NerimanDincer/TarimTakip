using Microsoft.EntityFrameworkCore;
using TarimTakip.API.Data;
using TarimTakip.API.Data.Entities;
using TarimTakip.API.Models.DTOs.Plant;

namespace TarimTakip.API.Services
{
    public class PlantInfoService : IPlantInfoService
    {
        private readonly ApplicationDbContext _context;

        public PlantInfoService(ApplicationDbContext context)
        {
            _context = context;
        }

        // HERKESE AÇIK: Tüm bitkileri listele
        public async Task<List<PlantInfoResponseDto>> GetAllPlantsAsync()
        {
            return await _context.PlantInfos
                .Select(p => new PlantInfoResponseDto
                {
                    Id = p.Id,
                    PlantName = p.PlantName,
                    SoilPreparation = p.SoilPreparation,
                    WaterNeeds = p.WaterNeeds,
                    HarvestTime = p.HarvestTime
                })
                .ToListAsync();
        }

        // HERKESE AÇIK: Tek bitkiyi getir
        public async Task<PlantInfoResponseDto> GetPlantByIdAsync(int id)
        {
            var plant = await _context.PlantInfos
                .Select(p => new PlantInfoResponseDto
                {
                    Id = p.Id,
                    PlantName = p.PlantName,
                    SoilPreparation = p.SoilPreparation,
                    WaterNeeds = p.WaterNeeds,
                    HarvestTime = p.HarvestTime
                })
                .FirstOrDefaultAsync(p => p.Id == id);

            if (plant == null)
                throw new Exception("Bitki bulunamadı.");

            return plant;
        }

        // SADECE ADMIN: Bitki oluştur
        public async Task<PlantInfoResponseDto> CreatePlantAsync(PlantInfoCreateDto request)
        {
            // Bu isimde bitki var mı? (Region'da yaptığımız kontrolün aynısı)
            var exists = await _context.PlantInfos.AnyAsync(p => p.PlantName.ToUpper() == request.PlantName.ToUpper());
            if (exists)
                throw new Exception("Bu bitki adı zaten kayıtlı.");

            var plant = new PlantInfo
            {
                PlantName = request.PlantName,
                SoilPreparation = request.SoilPreparation,
                WaterNeeds = request.WaterNeeds,
                HarvestTime = request.HarvestTime
            };

            _context.PlantInfos.Add(plant);
            await _context.SaveChangesAsync();

            return await GetPlantByIdAsync(plant.Id);
        }

        // SADECE ADMIN: Bitki güncelle
        public async Task UpdatePlantAsync(int id, PlantInfoCreateDto request)
        {
            var plant = await _context.PlantInfos.FindAsync(id);
            if (plant == null)
                throw new Exception("Bitki bulunamadı.");

            // "Akıllı Güncelleme" (PATCH Mantığı)
            // Sadece DTO'da GÖNDERİLEN (null olmayan) alanları güncelle.

            // PlantName DTO'da [Required] olduğu için bunun dolu geleceğini varsayıyoruz.
            plant.PlantName = request.PlantName;

            // Diğer alanları kontrol et:
            // Eğer request.SoilPreparation 'null' DEĞİLSE güncelle.
            // 'null' ise, veritabanındaki eski değere dokunma.
            if (request.SoilPreparation != null)
            {
                plant.SoilPreparation = request.SoilPreparation;
            }

            if (request.WaterNeeds != null)
            {
                plant.WaterNeeds = request.WaterNeeds;
            }

            if (request.HarvestTime != null)
            {
                plant.HarvestTime = request.HarvestTime;
            }

            _context.PlantInfos.Update(plant);
            await _context.SaveChangesAsync();
        }

        // SADECE ADMIN: Bitki sil
        public async Task DeletePlantAsync(int id)
        {
            var plant = await _context.PlantInfos.FindAsync(id);
            if (plant == null)
                throw new Exception("Bitki bulunamadı.");

            _context.PlantInfos.Remove(plant);

            // Eğer bu bitkiye bağlı PlantRegion varsa, Adım 1'deki kuralımız
            // buranın hata vermesini sağlayacak (FOREIGN KEY kısıtlaması).
            // Bu istediğimiz bir şey!
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException) // Veritabanı hatasını yakala
            {
                throw new Exception("Bu bitki silinemez çünkü bölgesel kayıtlara (PlantRegion) bağlıdır.");
            }
        }
    }
}