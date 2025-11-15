using System.ComponentModel.DataAnnotations;
//Bu DTO'da UserId istemedik. Çünkü kullanıcı zaten giriş yapmış olacak, UserId bilgisini bize gelen Token'dan (kimlik kartından) biz alacağız.
namespace TarimTakip.API.Models.DTOs.Farm
{
    public class FarmFieldCreateDto
    {
        [Required]
        public string PlantName { get; set; }

        [Required]
        public DateTime SowingDate { get; set; }

        [Required]
        public decimal Area { get; set; }

        public string? SoilInfo { get; set; }

        [Required]
        public int RegionId { get; set; }
    }
}