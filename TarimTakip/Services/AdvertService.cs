using Microsoft.EntityFrameworkCore;
using TarimTakip.API.Data;
using TarimTakip.API.Data.Entities;
using TarimTakip.API.Models.DTOs.Advert;

namespace TarimTakip.API.Services
{
    public class AdvertService : IAdvertService
    {
        private readonly ApplicationDbContext _context;

        public AdvertService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateAdvertAsync(AdvertCreateDto request, int sellerId)
        {
            var advert = new Advert
            {
                Title = request.Title,
                Description = request.Description,
                Price = request.Price,
                Unit = request.Unit,
                Quantity = request.Quantity,
                ImageUrl = request.ImageUrl,
                SellerId = sellerId, // Token'dan gelen ID
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await _context.Adverts.AddAsync(advert);
            await _context.SaveChangesAsync();
        }

        public async Task<List<AdvertResponseDto>> GetAllAdvertsAsync()
        {
            // Sadece Aktif ilanları getir, Satıcısını da dahil et (Include)
            var adverts = await _context.Adverts
                .Include(a => a.Seller)
                .Where(a => a.IsActive == true)
                .OrderByDescending(a => a.CreatedAt) // En yeni en üstte
                .ToListAsync();

            return adverts.Select(a => new AdvertResponseDto
            {
                Id = a.Id,
                Title = a.Title,
                Description = a.Description,
                Price = a.Price,
                Unit = a.Unit,
                Quantity = a.Quantity,
                ImageUrl = a.ImageUrl,
                SellerName = a.Seller.FullName, // İlişkili tablodan isim aldık
                SellerId = a.SellerId,
                CreatedAt = a.CreatedAt
            }).ToList();
        }
    }
}