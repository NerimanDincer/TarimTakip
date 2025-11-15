namespace TarimTakip.API.Models.DTOs.Chat
{
    public class MessageResponseDto
    {
        public int Id { get; set; }
        public int ChatRoomId { get; set; }
        public int SenderId { get; set; }
        public string SenderName { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}