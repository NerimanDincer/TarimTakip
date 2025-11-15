namespace TarimTakip.API.Models.DTOs.Admin
{
    public class UserResponseDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public string Role { get; set; }
        public string RegionName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}