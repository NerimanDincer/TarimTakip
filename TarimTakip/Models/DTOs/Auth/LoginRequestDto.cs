using System.ComponentModel.DataAnnotations;

namespace TarimTakip.API.Models.DTOs.Auth
{
    public class LoginRequestDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}