using TarimTakip.API.Models.DTOs.Advert;

namespace TarimTakip.API.Services
{
    public interface IAdvertService
    {
        Task CreateAdvertAsync(AdvertCreateDto request, int sellerId);
        Task<List<AdvertResponseDto>> GetAllAdvertsAsync();
    }
}