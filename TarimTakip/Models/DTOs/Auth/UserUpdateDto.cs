namespace TarimTakip.API.Models.DTOs.Auth
{
    public class UserUpdateDto
    {
        public string FullName { get; set; } // Adını değiştirmek isteyebilir
        public string? Bio { get; set; }
        public string? ProfilePictureUrl { get; set; } // ImageController'dan dönen URL buraya gelecek
        public int RegionId { get; set; } // Taşınırsa bölgesini değiştirsin
    }
}