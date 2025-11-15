using TarimTakip.API.Data;
using TarimTakip.API.Data.Entities;
using TarimTakip.API.Models.DTOs.Farm;

namespace TarimTakip.API.Services
{
    public class SaleService : ISaleService
    {
        private readonly ApplicationDbContext _context;

        public SaleService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateSaleAsync(int farmFieldId, SaleCreateDto request, int userId)
        {
            // 1. GÜVENLİK KONTROLÜ
            var farmField = await _context.FarmFields.FindAsync(farmFieldId);

            if (farmField == null)
            {
                throw new Exception("Tarla bulunamadı.");
            }

            if (farmField.UserId != userId)
            {
                throw new Exception("Bu tarlaya satış (gelir) kaydı ekleme yetkiniz yok.");
            }

            // 2. KAYIT OLUŞTURMA
            var sale = new Sale
            {
                FarmFieldId = farmFieldId,
                AmountKg = request.AmountKg,
                Price = request.Price,
                Date = request.Date
            };

            await _context.Sales.AddAsync(sale);
            await _context.SaveChangesAsync();
        }
    }
}