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
        public async Task<AdminStatsDto> GetAdminStatsAsync()
        {
            // 1. Sorguları PARALEL değil, SIRAYLA çalıştırıyoruz.
            var farmerCount = await _context.Users.CountAsync(u => u.Role == "Farmer");
            var engineerCount = await _context.Users.CountAsync(u => u.Role == "Engineer");
            var pendingQuestions = await _context.Questions.CountAsync(q => q.Status == "Pending");

            // 2. Bu ayın başlangıcını hesapla
            var today = DateTime.UtcNow;
            var startOfMonth = new DateTime(today.Year, today.Month, 1);

            // 3. Sırayla devam et
            var monthlyExpenses = await _context.Expenses
                .Where(e => e.Date >= startOfMonth)
                .SumAsync(e => e.Amount);

            var monthlyIncome = await _context.Sales
                .Where(s => s.Date >= startOfMonth)
                .SumAsync(s => s.Price);

            // 4. Sonuçları DTO'ya doldur
            var stats = new AdminStatsDto
            {
                TotalFarmers = farmerCount,
                TotalEngineers = engineerCount,
                PendingQuestions = pendingQuestions,
                TotalExpensesThisMonth = monthlyExpenses,
                TotalIncomeThisMonth = monthlyIncome
            };

            // Toplam kullanıcı sayısını ayriyeten hesapla
            stats.TotalUsers = stats.TotalFarmers + stats.TotalEngineers + 1; // +1 (Admin'in kendisi)

            return stats;
        }
        public async Task<bool> ToggleUserStatusAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new Exception("Kullanıcı bulunamadı.");
            }

            if (user.Role == "Admin")
            {
                throw new Exception("Admin kullanıcısının durumu değiştirilemez.");
            }

            // Durumu tersine çevir
            // Eğer 'true' ise 'false' yap, 'false' ise 'true' yap.
            user.IsActive = !user.IsActive;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return user.IsActive; // Kullanıcının yeni durumunu döndür
        }
        public async Task UpdateUserRoleAsync(int userId, string newRole)
        {
            // 1. Yeni rol geçerli mi?
            if (newRole != "Farmer" && newRole != "Engineer")
            {
                throw new Exception("Geçersiz rol. Rol sadece 'Farmer' veya 'Engineer' olabilir.");
            }

            // 2. Kullanıcıyı bul
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new Exception("Kullanıcı bulunamadı.");
            }

            // 3. Admin'in rolü değiştirilemez
            if (user.Role == "Admin")
            {
                throw new Exception("Admin kullanıcısının rolü değiştirilemez.");
            }

            // 4. Rolü güncelle
            user.Role = newRole;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}