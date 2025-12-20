using System.Text.Json.Serialization;

namespace TarimTakip.API.Models.DTOs.Weather
{
    // OpenWeatherMap'ten gelen karmaşık veriyi karşılayan yardımcı sınıflar
    public class OpenWeatherMapResult
    {
        [JsonPropertyName("weather")]
        public List<WeatherInfo> Weather { get; set; }

        [JsonPropertyName("main")]
        public MainInfo Main { get; set; }

        [JsonPropertyName("wind")]
        public WindInfo Wind { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class WeatherInfo
    {
        [JsonPropertyName("description")]
        public string Description { get; set; }
    }

    public class MainInfo
    {
        [JsonPropertyName("temp")]
        public double Temp { get; set; }

        [JsonPropertyName("humidity")]
        public int Humidity { get; set; }
    }

    public class WindInfo
    {
        [JsonPropertyName("speed")]
        public double Speed { get; set; }
    }
}