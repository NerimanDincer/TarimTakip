using TarimTakip.API.Models.DTOs.Blog;

namespace TarimTakip.API.Services
{
    public interface IBlogPostService
    {
        // Tüm yazıları getir (halka açık)
        Task<List<BlogPostResponseDto>> GetAllPostsAsync();

        // Yeni yazı oluştur (güvenli)
        Task CreatePostAsync(BlogPostCreateDto request, int engineerId);
        Task DeletePostAsync(int postId);
    }
}