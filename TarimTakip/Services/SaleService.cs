using Microsoft.EntityFrameworkCore;
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
            var field = await _context.FarmFields.FirstOrDefaultAsync(f => f.Id == farmFieldId && f.UserId == userId);
            if (field == null) throw new Exception("Tarla bulunamadı veya yetkiniz yok.");

            var sale = new Sale
            {
                FarmFieldId = farmFieldId,
                AmountKg = request.AmountKg,
                UnitPrice = request.UnitPrice,
                TotalPrice = request.AmountKg * request.UnitPrice, // Toplamı otomatik hesapla
                Date = request.Date,
                IsDeleted = false,
                CreatedDate = DateTime.UtcNow
            };

            await _context.Sales.AddAsync(sale);
            await _context.SaveChangesAsync();
        }

        // LİSTELEME (Sadece silinmemiş olanları getirir)
        public async Task<List<object>> GetSalesByFieldAsync(int farmFieldId, int userId)
        {
            var field = await _context.FarmFields.FirstOrDefaultAsync(f => f.Id == farmFieldId && f.UserId == userId);
            if (field == null) throw new Exception("Tarla bulunamadı veya yetkiniz yok.");

            var sales = await _context.Sales
                .Where(s => s.FarmFieldId == farmFieldId && !s.IsDeleted) // Soft delete kontrolü
                .OrderByDescending(s => s.Date)
                .Select(s => new
                {
                    s.Id,
                    s.AmountKg,
                    s.UnitPrice,
                    s.TotalPrice,
                    s.Date
                }).ToListAsync<object>();

            return sales;
        }

        // GÜNCELLEME
        public async Task UpdateSaleAsync(int saleId, SaleCreateDto request, int userId)
        {
            var sale = await _context.Sales.Include(s => s.FarmField)
                .FirstOrDefaultAsync(s => s.Id == saleId && s.FarmField.UserId == userId && !s.IsDeleted);

            if (sale == null) throw new Exception("Satış kaydı bulunamadı veya yetkiniz yok.");

            sale.AmountKg = request.AmountKg;
            sale.UnitPrice = request.UnitPrice;
            sale.TotalPrice = request.AmountKg * request.UnitPrice;
            sale.Date = request.Date;

            _context.Sales.Update(sale);
            await _context.SaveChangesAsync();
        }

        // SİLME (Soft Delete)
        public async Task DeleteSaleAsync(int saleId, int userId)
        {
            var sale = await _context.Sales.Include(s => s.FarmField)
                .FirstOrDefaultAsync(s => s.Id == saleId && s.FarmField.UserId == userId);

            if (sale == null) throw new Exception("Satış kaydı bulunamadı.");

            sale.IsDeleted = true; // Gerçekten silme, sadece çöp kutusuna at
            _context.Sales.Update(sale);
            await _context.SaveChangesAsync();
        }
    }
}