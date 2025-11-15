using System.Threading.Tasks;
using TarimTakip.API.Models.DTOs.Admin;

namespace TarimTakip.API.Services
{
    public interface IAdminService
    {
        Task<List<UserResponseDto>> GetAllUsersAsync();
        Task DeleteUserAsync(int userId);
    }
}