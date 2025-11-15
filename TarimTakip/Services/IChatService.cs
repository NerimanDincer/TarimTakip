using TarimTakip.API.Models.DTOs.Chat;

namespace TarimTakip.API.Services
{
    public interface IChatService
    {
        // Bir çiftçinin mühendisle olan sohbetini bul veya oluştur
        Task<ChatRoomResponseDto> FindOrCreateChatRoomAsync(int farmerId, int engineerId);

        // Bir çiftçinin tüm sohbet odalarını listele
        Task<List<ChatRoomResponseDto>> GetChatRoomsForUserAsync(int userId);
    }
}