using Microsoft.EntityFrameworkCore; // .Include() ve .Select() için gerekli
using TarimTakip.API.Data;
using TarimTakip.API.Data.Entities;
using TarimTakip.API.Models.DTOs.Blog;

namespace TarimTakip.API.Services
{
    public class BlogPostService : IBlogPostService
    {
        private readonly ApplicationDbContext _context;

        public BlogPostService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<BlogPostResponseDto>> GetAllPostsAsync()
        {
            // Veritabanından veriyi çekerken, ilişkili olduğu Engineer (User)
            // bilgisini de .Include() ile çekiyoruz.
            // Sonra .Select() ile DTO'muza dönüştürüyoruz.
            return await _context.BlogPosts
                .Include(bp => bp.Engineer) // Engineer (User) tablosuna JOIN atar
                .OrderByDescending(bp => bp.CreatedAt) // En yeni yazı en üstte
                .Select(bp => new BlogPostResponseDto
                {
                    Id = bp.Id,
                    Title = bp.Title,
                    Content = bp.Content,
                    CreatedAt = bp.CreatedAt,
                    EngineerName = bp.Engineer.FullName // İlişkili tablodan adı al
                })
                .ToListAsync();
        }

        public async Task CreatePostAsync(BlogPostCreateDto request, int engineerId)
        {
            var newPost = new BlogPost
            {
                Title = request.Title,
                Content = request.Content,
                EngineerId = engineerId, // Token'dan gelen ID
                CreatedAt = DateTime.UtcNow
            };

            await _context.BlogPosts.AddAsync(newPost);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePostAsync(int postId)
        {
            var post = await _context.BlogPosts.FindAsync(postId);
            if (post == null)
            {
                throw new Exception("Blog yazısı bulunamadı.");
            }

            _context.BlogPosts.Remove(post);
            await _context.SaveChangesAsync();
        }
    }
}