using Microsoft.AspNetCore.Mvc;
using TarimTakip.API.Services;

namespace TarimTakip.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherService _weatherService;

        public WeatherController(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        // GET: /api/weather?city=Antalya
        [HttpGet]
        public async Task<IActionResult> GetWeather([FromQuery] string city)
        {
            if (string.IsNullOrEmpty(city))
                return BadRequest("Şehir adı gereklidir.");

            try
            {
                var result = await _weatherService.GetWeatherAsync(city);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}