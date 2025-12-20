using System.Text.Json; // JSON işlemleri için
using TarimTakip.API.Models.DTOs.Weather;

namespace TarimTakip.API.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        // HttpClient: İnternete çıkış kapımız (Browser gibi davranır)
        public WeatherService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<WeatherResponseDto> GetWeatherAsync(string cityName)
        {
            // 1. Ayarları al
            var apiKey = _configuration["OpenWeatherMap:ApiKey"];
            var baseUrl = _configuration["OpenWeatherMap:BaseUrl"];

            // 2. İsteği hazırla (metric = Celcius olsun diye)
            // Örn: https://api.openweathermap.org/data/2.5/weather?q=Antalya&appid=12345&units=metric&lang=tr
            var url = $"{baseUrl}weather?q={cityName}&appid={apiKey}&units=metric&lang=tr";

            // 3. İsteği gönder
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                // Şehir bulunamazsa veya API hata verirse
                throw new Exception($"Hava durumu alınamadı: {response.ReasonPhrase}");
            }

            // 4. Gelen cevabı oku
            var jsonString = await response.Content.ReadAsStringAsync();

            // 5. JSON'u C# nesnesine çevir
            var weatherData = JsonSerializer.Deserialize<OpenWeatherMapResult>(jsonString);

            // 6. Bizim temiz modelimize dönüştür
            return new WeatherResponseDto
            {
                City = weatherData.Name,
                Temperature = weatherData.Main.Temp,
                Description = weatherData.Weather.FirstOrDefault()?.Description ?? "Bilinmiyor",
                Humidity = weatherData.Main.Humidity,
                WindSpeed = weatherData.Wind.Speed
            };
        }
    }
}