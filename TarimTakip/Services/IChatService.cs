using TarimTakip.API.Models.DTOs.Chat;

namespace TarimTakip.API.Services
{
    public interface IChatService
    {
        Task<ChatRoomResponseDto> FindOrCreateChatRoomAsync(int farmerId, int engineerId);
        Task<List<ChatRoomResponseDto>> GetChatRoomsForUserAsync(int userId);
        Task<MessageResponseDto> SendMessageAsync(int chatRoomId, int senderId, string text);
    }
}