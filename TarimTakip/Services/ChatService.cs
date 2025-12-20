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
        public async Task<MessageResponseDto> SendMessageAsync(int chatRoomId, int senderId, string text)
        {
            // 1. Odayı bul
            var room = await _context.ChatRooms.FindAsync(chatRoomId);
            if (room == null)
                throw new Exception("Sohbet odası bulunamadı.");

            // 2. GÜVENLİK KONTROLÜ: 
            // Mesajı atan kişi bu odanın Çiftçisi mi? Veya Mühendisi mi?
            // Eğer ikisi de değilse, bu odaya mesaj atamaz!
            if (room.FarmerId != senderId && room.EngineerId != senderId)
            {
                throw new Exception("Bu sohbet odasına mesaj gönderme yetkiniz yok.");
            }

            // 3. Mesajı oluştur
            var message = new Data.Entities.Message
            {
                ChatRoomId = chatRoomId,
                SenderId = senderId,
                Text = text,
                CreatedAt = DateTime.UtcNow
            };

            // 4. Kaydet
            await _context.Messages.AddAsync(message);
            await _context.SaveChangesAsync();

            // 5. Gönderenin adını da bulalım (Geri dönerken lazım olacak)
            var sender = await _context.Users.FindAsync(senderId);

            // 6. DTO olarak geri döndür
            return new MessageResponseDto
            {
                Id = message.Id,
                ChatRoomId = message.ChatRoomId,
                SenderId = message.SenderId,
                SenderName = sender.FullName,
                Text = message.Text,
                CreatedAt = message.CreatedAt
            };
        }
    }
}