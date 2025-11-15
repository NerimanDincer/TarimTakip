namespace TarimTakip.API.Models.DTOs.Chat
{
    public class ChatRoomResponseDto
    {
        public int Id { get; set; }
        public int FarmerId { get; set; }
        public string FarmerName { get; set; }
        public int EngineerId { get; set; }
        public string EngineerName { get; set; }
        public List<MessageResponseDto> Messages { get; set; }
    }
}