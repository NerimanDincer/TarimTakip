using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TarimTakip.API.Services;

namespace TarimTakip.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageService _imageService;

        public ImageController(IImageService imageService)
        {
            _imageService = imageService;
        }

        // POST: /api/Image/upload
        [HttpPost("upload")]
        [Authorize] // Sadece giriş yapmış kullanıcılar resim yükleyebilir
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            try
            {
                var imageUrl = await _imageService.SaveImageAsync(file);
                return Ok(new { Url = imageUrl });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}