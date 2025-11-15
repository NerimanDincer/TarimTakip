namespace TarimTakip.API.Models.DTOs.QA
{
    public class AnswerResponseDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public string EngineerName { get; set; }
    }
}