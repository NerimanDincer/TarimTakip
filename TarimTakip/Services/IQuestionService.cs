using TarimTakip.API.Models.DTOs.QA;

namespace TarimTakip.API.Services
{
    public interface IQuestionService
    {
        Task<List<QuestionResponseDto>> GetAllQuestionsAsync();
        Task<QuestionDetailDto> GetQuestionDetailAsync(int questionId);
        Task<int> CreateQuestionAsync(QuestionCreateDto request, int farmerId);
        Task<AnswerResponseDto> CreateAnswerAsync(int questionId, AnswerCreateDto request, int engineerId);
    }
}