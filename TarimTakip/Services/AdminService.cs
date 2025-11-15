using Microsoft.EntityFrameworkCore;
using TarimTakip.API.Data;
using TarimTakip.API.Models.DTOs.Admin;

namespace TarimTakip.API.Services
{
    public class AdminService : IAdminService
    {
        private readonly ApplicationDbContext _context;

        public AdminService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<UserResponseDto>> GetAllUsersAsync()
        {
            // Tüm kullanıcıları, bölge adlarıyla birlikte çek
            return await _context.Users
                .Include(u => u.Region) // Region tablosuna JOIN at
                .OrderBy(u => u.Id)
                .Select(u => new UserResponseDto
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    Email = u.Email,
                    Phone = u.Phone,
                    Role = u.Role,
                    RegionName = u.Region.Name, // İlişkili tablodan adı al
                    CreatedAt = u.CreatedAt
                })
                .ToListAsync();
        }

        public async Task DeleteUserAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new Exception("Kullanıcı bulunamadı.");
            }

            if (user.Role == "Admin")
            {
                throw new Exception("Admin kullanıcıları sistemden silinemez.");
            }

            // --- YENİ EKLENEN AKILLI SİLME KISMI ---

            // 1. Kullanıcının dahil olduğu ÇOKLU YOL (ChatRoom) kayıtlarını bul ve sil
            var chatRooms = await _context.ChatRooms
                .Where(c => c.FarmerId == userId || c.EngineerId == userId)
                .ToListAsync();

            if (chatRooms.Any())
            {
                _context.ChatRooms.RemoveRange(chatRooms);
            }

            // 2. Kullanıcının dahil olduğu ÇOKLU YOL (Answer) kayıtlarını bul ve sil
            if (user.Role == "Engineer")
            {
                var answers = await _context.Answers
                    .Where(a => a.EngineerId == userId)
                    .ToListAsync();

                if (answers.Any())
                {
                    _context.Answers.RemoveRange(answers);
                }
            }
            // --- YENİ KISIM SONU ---

            // 3. Kullanıcıyı sil
            // (Diğer kayıtlar - Tarlalar, Sorular, Bloglar - 'Cascade' olarak ayarlandığı
            // veya bu kayıtlarla ilişkili olmadığı için otomatik silinecek)
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}