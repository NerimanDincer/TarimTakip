using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TarimTakip.API.Models.DTOs.QA;
using TarimTakip.API.Services;

namespace TarimTakip.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionService _questionService;

        public QuestionController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        // HERKESE AÇIK
        // GET: /api/Question
        [HttpGet]
        public async Task<IActionResult> GetAllQuestions()
        {
            var questions = await _questionService.GetAllQuestionsAsync();
            return Ok(questions);
        }

        // HERKESE AÇIK
        // GET: /api/Question/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetQuestionDetail(int id)
        {
            try
            {
                var questionDetail = await _questionService.GetQuestionDetailAsync(id);
                return Ok(questionDetail);
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        // SADECE ÇİFTÇİLER
        // POST: /api/Question
        [HttpPost]
        [Authorize(Roles = "Farmer")]
        public async Task<IActionResult> CreateQuestion([FromBody] QuestionCreateDto request)
        {
            var farmerId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var questionId = await _questionService.CreateQuestionAsync(request, farmerId);
            return Ok(new { Message = "Soru başarıyla oluşturuldu.", QuestionId = questionId });
        }

        // SADECE MÜHENDİSLER
        // POST: /api/Question/1/answer
        [HttpPost("{questionId}/answer")]
        [Authorize(Roles = "Engineer")]
        public async Task<IActionResult> CreateAnswer(int questionId, [FromBody] AnswerCreateDto request)
        {
            try
            {
                var engineerId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var answer = await _questionService.CreateAnswerAsync(questionId, request, engineerId);
                return Ok(answer);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}