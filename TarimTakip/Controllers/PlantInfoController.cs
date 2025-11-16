using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TarimTakip.API.Models.DTOs.Plant;
using TarimTakip.API.Services;

namespace TarimTakip.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlantInfoController : ControllerBase
    {
        private readonly IPlantInfoService _plantInfoService;

        public PlantInfoController(IPlantInfoService plantInfoService)
        {
            _plantInfoService = plantInfoService;
        }

        // --- HALKA AÇIK UÇ NOKTALAR ---

        // GET: /api/PlantInfo
        [HttpGet]
        public async Task<IActionResult> GetAllPlants()
        {
            var plants = await _plantInfoService.GetAllPlantsAsync();
            return Ok(plants);
        }

        // GET: /api/PlantInfo/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlantById(int id)
        {
            try
            {
                var plant = await _plantInfoService.GetPlantByIdAsync(id);
                return Ok(plant);
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        // --- SADECE ADMIN UÇ NOKTALARI ---

        // POST: /api/PlantInfo
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreatePlant([FromBody] PlantInfoCreateDto request)
        {
            try
            {
                var newPlant = await _plantInfoService.CreatePlantAsync(request);
                return Ok(newPlant);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // PUT: /api/PlantInfo/1
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdatePlant(int id, [FromBody] PlantInfoCreateDto request)
        {
            try
            {
                await _plantInfoService.UpdatePlantAsync(id, request);
                return Ok(new { Message = "Bitki bilgisi başarıyla güncellendi." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message }); // "Bulunamadı" veya "Zaten var"
            }
        }

        // DELETE: /api/PlantInfo/1
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletePlant(int id)
        {
            try
            {
                await _plantInfoService.DeletePlantAsync(id);
                return Ok(new { Message = "Bitki başarıyla silindi." });
            }
            catch (Exception ex)
            {
                // "Bulunamadı" veya "Silinemez" hatası
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}