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

        // Yeni bölge ekleyecek
        [HttpPost]
        public async Task<IActionResult> CreateRegion([FromQuery] string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest("Bölge adı boş olamaz.");
            }

            try
            {
                // Artık bu metot bize oluşturulan bölgeyi döndürüyor
                var newRegion = await _regionService.CreateRegionAsync(name);

                // Başarılı olursa, oluşturulan bölgeyi JSON olarak döndür
                return Ok(newRegion);
            }
            catch (Exception ex)
            {
                // Serviste fırlattığımız "Bu bölge adı zaten kayıtlı." hatasını yakala
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}