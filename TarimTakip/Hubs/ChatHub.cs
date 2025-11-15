using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using TarimTakip.API.Data;
using TarimTakip.API.Data.Entities;

namespace TarimTakip.API.Hubs
{
    [Authorize] // Sadece giriş yapmış, token'ı olanlar bu Hub'a bağlanabilir
    public class ChatHub : Hub
    {
        private readonly ApplicationDbContext _context;

        public ChatHub(ApplicationDbContext context)
        {
            _context = context;
        }

        // Mobil uygulama bu metodu çağırarak odaya katılır
        public async Task JoinRoom(string chatRoomId)
        {
            // 'Groups.AddToGroupAsync' SignalR'ın "oda" mekanizmasıdır.
            // O odadaki (gruptaki) herkese mesaj yollamamızı sağlar.
            await Groups.AddToGroupAsync(Context.ConnectionId, chatRoomId);
        }

        // Mobil uygulama bu metodu çağırarak mesaj gönderir
        public async Task SendMessage(int chatRoomId, string text)
        {
            // 1. Mesajı kim gönderdi? (Token'dan al)
            var userId = int.Parse(Context.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var user = await _context.Users.FindAsync(userId);

            // 2. Mesajı veritabanına kaydet
            var message = new Message
            {
                ChatRoomId = chatRoomId,
                SenderId = userId,
                Text = text,
                CreatedAt = DateTime.UtcNow
            };

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            // 3. Mesajı odadaki DİĞER KİŞİLERE (ve kendine) geri yolla
            // Mobil uygulama "ReceiveMessage" adlı olayı dinleyecek
            await Clients.Group(chatRoomId.ToString()).SendAsync("ReceiveMessage", new
            {
                // DTO'yu burada manuel oluşturup yolluyoruz
                Id = message.Id,
                ChatRoomId = message.ChatRoomId,
                SenderId = message.SenderId,
                SenderName = user.FullName,
                Text = message.Text,
                CreatedAt = message.CreatedAt
            });
        }
    }
}