namespace TarimTakip.API.Models.DTOs.Weather
{
    // Bizim çiftçiye göstereceğimiz temiz model
    public class WeatherResponseDto
    {
        public string City { get; set; } // Şehir (Antalya)
        public double Temperature { get; set; } // Sıcaklık (24.5)
        public string Description { get; set; } // Durum (Parçalı Bulutlu)
        public int Humidity { get; set; } // Nem (%40)
        public double WindSpeed { get; set; } // Rüzgar Hızı
    }
}