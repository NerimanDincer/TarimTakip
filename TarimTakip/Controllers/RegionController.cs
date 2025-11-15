using Microsoft.AspNetCore.Mvc;
using TarimTakip.API.Services;

namespace TarimTakip.API.Controllers
{
    [Route("api/[controller]")] // URL: /api/region
    [ApiController]
    public class RegionController : ControllerBase
    {
        private readonly IRegionService _regionService;

        // 'RegionService'imizi DI ile alıyoruz
        public RegionController(IRegionService regionService)
        {
            _regionService = regionService;
        }

        // GET: /api/region
        // Tüm bölgeleri listeleyecek
        [HttpGet]
        public async Task<IActionResult> GetAllRegions()
        {
            var regions = await _regionService.GetAllRegionsAsync();
            return Ok(regions); // Bölgeleri JSON olarak döndür
        }

        // POST: /api/region?name=Antalya
        // Yeni bölge ekleyecek
        [HttpPost]
        public async Task<IActionResult> CreateRegion([FromQuery] string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest("Bölge adı boş olamaz.");
            }

            await _regionService.CreateRegionAsync(name);
            return Ok(new { Message = $"{name} bölgesi başarıyla eklendi." });
        }
    }
}