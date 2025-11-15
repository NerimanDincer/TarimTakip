using Microsoft.EntityFrameworkCore;
using TarimTakip.API.Data;
using TarimTakip.API.Data.Entities;
using TarimTakip.API.Models.DTOs.Chat;

namespace TarimTakip.API.Services
{
    public class ChatService : IChatService
    {
        private readonly ApplicationDbContext _context;

        public ChatService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ChatRoomResponseDto> FindOrCreateChatRoomAsync(int farmerId, int engineerId)
        {
            // Bu iki kullanıcı arasında zaten bir oda var mı?
            var room = await _context.ChatRooms
                .Include(c => c.Farmer)
                .Include(c => c.Engineer)
                .Include(c => c.Messages) // Eski mesajları da yükle
                    .ThenInclude(m => m.Sender) // Mesajı göndereni yükle
                .FirstOrDefaultAsync(c => c.FarmerId == farmerId && c.EngineerId == engineerId);

            if (room == null)
            {
                // Yoksa, yeni bir oda oluştur
                var farmer = await _context.Users.FindAsync(farmerId);
                var engineer = await _context.Users.FindAsync(engineerId);

                if (farmer == null || engineer == null)
                    throw new Exception("Kullanıcılar bulunamadı.");

                room = new ChatRoom
                {
                    FarmerId = farmerId,
                    EngineerId = engineerId,
                    CreatedAt = DateTime.UtcNow,
                    Farmer = farmer,
                    Engineer = engineer,
                    Messages = new List<Data.Entities.Message>()
                };

                await _context.ChatRooms.AddAsync(room);
                await _context.SaveChangesAsync();
            }

            // Odayı DTO'ya dönüştür ve döndür
            return MapChatRoomToDto(room);
        }

        public async Task<List<ChatRoomResponseDto>> GetChatRoomsForUserAsync(int userId)
        {
            // Kullanıcının (ister çiftçi ister mühendis olsun) dahil olduğu tüm odaları bul
            var rooms = await _context.ChatRooms
                .Include(c => c.Farmer)
                .Include(c => c.Engineer)
                .Where(c => c.FarmerId == userId || c.EngineerId == userId)
                .ToListAsync();

            return rooms.Select(MapChatRoomToDto).ToList();
        }

        // --- Yardımcı Metot ---
        private ChatRoomResponseDto MapChatRoomToDto(Data.Entities.ChatRoom room)
        {
            return new ChatRoomResponseDto
            {
                Id = room.Id,
                FarmerId = room.FarmerId,
                FarmerName = room.Farmer.FullName,
                EngineerId = room.EngineerId,
                EngineerName = room.Engineer.FullName,
                Messages = room.Messages?.Select(m => new MessageResponseDto
                {
                    Id = m.Id,
                    ChatRoomId = m.ChatRoomId,
                    SenderId = m.SenderId,
                    SenderName = m.Sender.FullName,
                    Text = m.Text,
                    CreatedAt = m.CreatedAt
                }).OrderBy(m => m.CreatedAt).ToList() ?? new List<MessageResponseDto>()
            };
        }
    }
}