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

            // 2. Kullanıcı yoksa veya parola yanlışsa
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash) || user.IsActive == false)
            {
                throw new Exception("Email veya parola hatalı.");
            }

            // 3. Parola doğruysa, Token oluştur
            var token = GenerateJwtToken(user);

            // 4. Token ve kullanıcı bilgisi döndür
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
    }
}
