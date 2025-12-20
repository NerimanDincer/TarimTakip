namespace TarimTakip.API.Models.DTOs.QA
{
    public class QuestionResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Status { get; set; } // "Pending", "Answered"
        public string FarmerName { get; set; }
        public DateTime CreatedAt { get; set; }
        public int AnswerCount { get; set; } // Kaç cevap gelmiş
        public string? ImageUrl { get; set; }
    }
}