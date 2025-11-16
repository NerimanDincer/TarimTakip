using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TarimTakip.API.Models.DTOs.Plant;
using TarimTakip.API.Services;

namespace TarimTakip.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlantRegionController : ControllerBase
    {
        private readonly IPlantRegionService _plantRegionService;

        public PlantRegionController(IPlantRegionService plantRegionService)
        {
            _plantRegionService = plantRegionService;
        }

        // --- SADECE ADMIN UÇ NOKTALARI (CRUD) ---

        // POST: /api/PlantRegion
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreatePlantRegion([FromBody] PlantRegionCreateDto request)
        {
            try
            {
                var newPlantRegion = await _plantRegionService.CreatePlantRegionAsync(request);
                return Ok(newPlantRegion);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // PUT: /api/PlantRegion/1
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdatePlantRegion(int id, [FromBody] PlantRegionCreateDto request)
        {
            try
            {
                await _plantRegionService.UpdatePlantRegionAsync(id, request);
                return Ok(new { Message = "Bölgesel bitki kaydı güncellendi." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // DELETE: /api/PlantRegion/1
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletePlantRegion(int id)
        {
            try
            {
                await _plantRegionService.DeletePlantRegionAsync(id);
                return Ok(new { Message = "Bölgesel bitki kaydı silindi." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // --- HALKA AÇIK UÇ NOKTALAR (HARİTA ÖZELLİĞİ İÇİN) ---

        // GET: /api/PlantRegion/plant/1
        // (ID: 1 olan bitkinin (Buğday) tüm bölgelerini getirir)
        [HttpGet("plant/{plantId}")]
        public async Task<IActionResult> GetRegionsForPlant(int plantId)
        {
            var regions = await _plantRegionService.GetRegionsForPlantAsync(plantId);
            return Ok(regions);
        }

        // GET: /api/PlantRegion/region/1002
        // (ID: 1002 olan bölgenin (Adana) tüm bitkilerini getirir)
        [HttpGet("region/{regionId}")]
        public async Task<IActionResult> GetPlantsForRegion(int regionId)
        {
            var plants = await _plantRegionService.GetPlantsForRegionAsync(regionId);
            return Ok(plants);
        }
    }
}
