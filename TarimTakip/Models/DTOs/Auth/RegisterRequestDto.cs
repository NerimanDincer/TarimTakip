using System.ComponentModel.DataAnnotations;

namespace TarimTakip.API.Models.DTOs.Auth
{
    public class RegisterRequestDto
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public string? Phone { get; set; }

        [Required]
        public string Role { get; set; } // "Farmer" veya "Engineer"

        [Required]
        public int RegionId { get; set; }
    }
}
