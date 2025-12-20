using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TarimTakip.API.Models.DTOs.Blog;
using TarimTakip.API.Services;

namespace TarimTakip.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostController : ControllerBase
    {
        private readonly IBlogPostService _blogPostService;

        public BlogPostController(IBlogPostService blogPostService)
        {
            _blogPostService = blogPostService;
        }

        // GET: /api/blogpost
        // Bu metot HALKA AÇIK. [Authorize] etiketi yok!
        // Herkes (giriş yapmayanlar dahil) blog yazılarını okuyabilir.
        [HttpGet]
        public async Task<IActionResult> GetAllBlogPosts()
        {
            var posts = await _blogPostService.GetAllPostsAsync();
            return Ok(posts);
        }


        // POST: /api/blogpost
        // Bu metot GÜVENLİ ve ROL KORUMALI.
        [HttpPost]
        [Authorize(Roles = "Admin,Engineer")] 
        public async Task<IActionResult> CreateBlogPost([FromBody] BlogPostCreateDto request)
        {
            // Token'dan (kimlik kartından) mühendisin ID'sini al
            var engineerIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (engineerIdClaim == null)
            {
                return Unauthorized();
            }

            var engineerId = int.Parse(engineerIdClaim.Value);

            await _blogPostService.CreatePostAsync(request, engineerId);

            return Ok(new { Message = "Blog yazısı başarıyla oluşturuldu." });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")] // <-- SİHİRLİ KİLİT!
        public async Task<IActionResult> DeleteBlogPost(int id)
        {
            try
            {
                // Bu metodu çağırmaya sadece Admin'in yetkisi var.
                await _blogPostService.DeletePostAsync(id);
                return Ok(new { Message = "Blog yazısı Admin tarafından başarıyla silindi." });
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }
    }
}
