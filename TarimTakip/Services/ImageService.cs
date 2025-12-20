using Microsoft.AspNetCore.Hosting; // Klasör yollarını bulmak için

namespace TarimTakip.API.Services
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _environment;

        public ImageService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string> SaveImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new Exception("Dosya seçilmedi.");
            }

            // 1. Dosya uzantısını kontrol et (Sadece resim olsun)
            var extension = Path.GetExtension(file.FileName).ToLower();
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };

            if (!allowedExtensions.Contains(extension))
            {
                throw new Exception("Sadece .jpg, .jpeg ve .png formatları kabul edilir.");
            }

            // 2. Benzersiz bir isim oluştur (Örn: guId-resim.jpg)
            var uniqueFileName = $"{Guid.NewGuid()}{extension}";

            // 3. Kaydedilecek klasör yolunu bul (wwwroot/images)
            var uploadsFolder = Path.Combine(_environment.WebRootPath, "images");

            // Klasör yoksa oluştur
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // 4. Dosyayı kaydet
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            // 5. Resmin erişim adresini (URL) döndür
            // Örn: /images/asdasd-123123.jpg
            return $"/images/{uniqueFileName}";
        }
    }
}