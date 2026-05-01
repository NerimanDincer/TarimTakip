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

        public async Task<Region> CreateRegionAsync(string regionName)
        {
            // 1. GÜVENLİK KONTROLÜ: İsim var mı? (Büyük/küçük harf duyarsız)
            var existingRegion = await _context.Regions
                .FirstOrDefaultAsync(r => r.Name.ToUpper() == regionName.ToUpper());

            if (existingRegion != null)
            {
                // Eğer bölge zaten varsa, HATA FIRLAT
                throw new Exception("Bu bölge adı zaten kayıtlı.");
            }

            // 2. Yeni bölgeyi oluştur
            var newRegion = new Region
            {
                Name = regionName // İsim büyük/küçük harf nasıl verildiyse öyle kaydet
            };

            await _context.Regions.AddAsync(newRegion);
            await _context.SaveChangesAsync();

            return newRegion; // Oluşturulan yeni bölgeyi döndür
        }
        public async Task UpdateRegionAsync(int id, string newName)
        {
            var region = await _context.Regions.FindAsync(id);
            if (region == null)
            {
                throw new Exception("Bölge bulunamadı.");
            }

            region.Name = newName; // İsmi güncelledik
            await _context.SaveChangesAsync(); // Veritabanına kaydettik
        }
    }
}