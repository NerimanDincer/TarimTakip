using TarimTakip.API.Models.DTOs.Weather;

namespace TarimTakip.API.Services
{
    public interface IWeatherService
    {
        Task<WeatherResponseDto> GetWeatherAsync(string cityName);
    }
}