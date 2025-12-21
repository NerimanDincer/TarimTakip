using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using TarimTakip.API.Services; // Service'i kullanmak için ekledik

namespace TarimTakip.API.Hubs
{
    [Authorize] // Sadece token'ı olanlar girebilir
    public class ChatHub : Hub
    {
        private readonly IChatService _chatService;

        // DbContext yerine Service enjekte ediyoruz.
        // Böylece kayıt mantığı tek bir yerde (Service) tutuluyor.
        public ChatHub(IChatService chatService)
        {
            _chatService = chatService;
        }

        // 1. Odaya Katılma (JoinRoom)
        public async Task JoinRoom(string chatRoomId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatRoomId);
        }

        // 2. Mesaj Gönderme (SendMessage)
        // Mobil uygulama soket üzerinden direkt mesaj atarsa burası çalışır
        public async Task SendMessage(int chatRoomId, string text)
        {
            // Kullanıcıyı bul
            var userId = int.Parse(Context.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            // İşi Service'e yaptır (Veritabanına kayıt)
            var messageDto = await _chatService.SendMessageAsync(chatRoomId, userId, text);

            // Sonucu odadaki herkese (Gönderen dahil) ilet
            await Clients.Group(chatRoomId.ToString()).SendAsync("ReceiveMessage", messageDto);
        }
    }
}