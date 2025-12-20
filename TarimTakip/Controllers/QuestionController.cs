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

        // PUT: /api/Question/5/assign
        // (ID: 5 olan soruyu bir mühendise ata)
        [HttpPut("{id}/assign")]
        [Authorize(Roles = "Admin")] // Sadece Admin yapabilir!
        public async Task<IActionResult> AssignQuestion(int id, [FromBody] AssignQuestionDto request)
        {
            try
            {
                await _questionService.AssignQuestionAsync(id, request.EngineerId);
                return Ok(new { Message = "Soru başarıyla mühendise atandı." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // --- MÜHENDİS İŞLEMLERİ ---

        // GET: /api/Question/my-assignments
        // (Mühendisin kendi sorularını listelemesi)
        [HttpGet("my-assignments")]
        [Authorize(Roles = "Engineer")] // Sadece Mühendisler girebilir
        public async Task<IActionResult> GetMyAssignments()
        {
            // Token'dan mühendisin kendi ID'sini buluyoruz (Sihirli kod)
            var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var engineerId = int.Parse(userIdString);

            var questions = await _questionService.GetQuestionsByEngineerAsync(engineerId);
            return Ok(questions);
        }
        // POST: /api/Question/5/answer
        // (ID: 5 olan soruya cevap yaz)
        [HttpPost("{id}/answer")]
        [Authorize(Roles = "Engineer")] // Sadece Mühendisler cevap yazabilir
        public async Task<IActionResult> AnswerQuestion(int id, [FromBody] AnswerCreateDto request)
        {
            try
            {
                // Mühendisin ID'sini token'dan alıyoruz
                var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                var engineerId = int.Parse(userIdString);

                var result = await _questionService.CreateAnswerAsync(id, request, engineerId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}