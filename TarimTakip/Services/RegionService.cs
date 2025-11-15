using Microsoft.EntityFrameworkCore;
using TarimTakip.API.Data;
using TarimTakip.API.Data.Entities;

namespace TarimTakip.API.Services
{
    public class RegionService : IRegionService
    {
        private readonly ApplicationDbContext _context;

        // DbContext'i 'Dependency Injection' ile alıyoruz
        public RegionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Region>> GetAllRegionsAsync()
        {
            // Veritabanındaki tüm Region kayıtlarını çek
            return await _context.Regions.ToListAsync();
        }

        public async Task CreateRegionAsync(string regionName)
        {
            // Yeni bir Region nesnesi oluştur
            var newRegion = new Region
            {
                Name = regionName
            };

            // Veritabanına ekle ve kaydet
            await _context.Regions.AddAsync(newRegion);
            await _context.SaveChangesAsync();
        }
    }
}