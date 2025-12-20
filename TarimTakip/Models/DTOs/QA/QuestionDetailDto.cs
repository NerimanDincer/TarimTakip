namespace TarimTakip.API.Models.DTOs.QA
{
    public class QuestionDetailDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Status { get; set; }
        public string FarmerName { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }

        // Soruya ait cevaplar
        public List<AnswerResponseDto> Answers { get; set; }
    }
}