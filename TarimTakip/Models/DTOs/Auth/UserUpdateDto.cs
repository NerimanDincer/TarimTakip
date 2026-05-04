namespace TarimTakip.API.Models.DTOs.Auth
{
    public class UserUpdateDto
    {
        public string FullName { get; set; }
        public string? Phone { get; set; } // İŞTE EKSİK PARÇAMIZ GELDİ!
        public string? Bio { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public int? RegionId { get; set; } // Soru işareti koyduk ki 0 olmasın, null gelsin
    }
}