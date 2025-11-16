using System.ComponentModel.DataAnnotations;

namespace TarimTakip.API.Models.DTOs.Admin
{
    public class AdminUserRoleUpdateDto
    {
        [Required]
        public string NewRole { get; set; } // "Farmer" veya "Engineer"
    }
}