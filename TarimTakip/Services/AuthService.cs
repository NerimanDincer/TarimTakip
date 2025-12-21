using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TarimTakip.API.Data;
using TarimTakip.API.Data.Entities;
using TarimTakip.API.Models.DTOs.Auth;

namespace TarimTakip.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;

        public AuthService(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config; // appsettings.json'ı okumak için
        }

        public async Task RegisterAsync(RegisterRequestDto request)
        {
            // 1. Email adresi zaten var mı?
            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
            {
                throw new Exception("Bu email adresi zaten kullanılıyor.");
            }

            // 2. Parolayı şifrele (Hash)
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            // 3. Yeni User nesnesi oluştur
            var user = new User
            {
                FullName = request.FullName,
                Email = request.Email,
                PasswordHash = hashedPassword, // Şifreli parolayı kaydet
                Phone = request.Phone,
                Role = request.Role,
                RegionId = request.RegionId,
                CreatedAt = DateTime.UtcNow
            };

            // 4. Veritabanına ekle
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            // Mühendis ise, boş profil oluştur
            if (user.Role == "Engineer")
            {
                var profile = new EngineerProfile { UserId = user.Id, CreatedAt = DateTime.UtcNow };
                await _context.EngineerProfiles.AddAsync(profile);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
        {
            // 1. Kullanıcıyı email ile bul
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

            // 2. KİMLİK KONTROLÜ: Kullanıcı yoksa veya parola yanlışsa
            // (Burada bilerek isActive kontrolünü yapmıyoruz)
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                throw new Exception("Email veya parola hatalı.");
            }

            // 3. DURUM KONTROLÜ: Şifre doğru, peki hesap aktif mi?
            // Eğer buraya geldiyse, şifre kesinlikle doğrudur.
            if (user.IsActive == false)
            {
                throw new Exception("Hesabınız yönetici tarafından askıya alınmıştır. Giriş yapamazsınız.");
            }

            // 4. Her şey yolunda, Token oluştur
            var token = GenerateJwtToken(user);

            return new LoginResponseDto
            {
                Id = user.Id,
                Token = token,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role
            };
        }


        // --- Token Oluşturma Metodu ---
        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            // appsettings'den gizli anahtarı al
            var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]);

            // Token'ın içine hangi bilgileri koyacağımızı seçiyoruz (Claims)
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()), // Kullanıcının ID'si
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Name, user.FullName),
                new Claim(ClaimTypes.Role, user.Role) // Kullanıcının Rolü
            };

            // Token'ın özelliklerini ayarla
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7), // Token 7 gün geçerli olsun
                Issuer = _config["Jwt:Issuer"],
                Audience = _config["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            // Token'ı oluştur
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // Token'ı string olarak döndür
            return tokenHandler.WriteToken(token);
        }
        // En alta ekleyebilirsin:

        public async Task<User> GetUserByIdAsync(int userId)
        {
            var user = await _context.Users.Include(u => u.Region).FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) throw new Exception("Kullanıcı bulunamadı.");
            return user;
        }

        public async Task UpdateProfileAsync(int userId, UserUpdateDto request)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) throw new Exception("Kullanıcı bulunamadı.");

            user.FullName = request.FullName;
            user.Bio = request.Bio;
            user.ProfilePictureUrl = request.ProfilePictureUrl;
            user.RegionId = request.RegionId;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
