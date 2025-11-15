using TarimTakip.API.Models.DTOs.Auth;

namespace TarimTakip.API.Services
{
    public interface IAuthService
    {
        Task RegisterAsync(RegisterRequestDto request);
        Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
    }
}