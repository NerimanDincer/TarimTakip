using Microsoft.AspNetCore.Http; 

namespace TarimTakip.API.Services
{
    public interface IImageService
    {
        Task<string> SaveImageAsync(IFormFile file);
    }
}