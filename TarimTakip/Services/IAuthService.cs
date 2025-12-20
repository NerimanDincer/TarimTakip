using TarimTakip.API.Data.Entities;
using TarimTakip.API.Models.DTOs.Auth;

namespace TarimTakip.API.Services
{
    public interface IAuthService
    {
        Task RegisterAsync(RegisterRequestDto request);
        Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
        Task UpdateProfileAsync(int userId, UserUpdateDto request);
        Task<User> GetUserByIdAsync(int userId); // Profilini görüntülemek için
    }
}