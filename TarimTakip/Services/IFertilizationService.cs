namespace TarimTakip.API.Services
{
    // DTO'yu bu interface'e eklemeye gerek yok, doğrudan Controller'da kullanabiliriz
    // Ama tutarlılık için ekleyelim.
    using TarimTakip.API.Models.DTOs.Farm;

    public interface IFertilizationService
    {
        Task CreateFertilizationAsync(int farmFieldId, FertilizationCreateDto request, int userId);
    }
}