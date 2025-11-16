using TarimTakip.API.Data.Entities;

namespace TarimTakip.API.Services
{
    public interface IRegionService
    {
        Task<List<Region>> GetAllRegionsAsync();
        Task<Region> CreateRegionAsync(string regionName);
    }
}
