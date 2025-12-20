using Microsoft.EntityFrameworkCore;
using TarimTakip.API.Data;
using TarimTakip.API.Data.Entities;
using TarimTakip.API.Models.DTOs.QA;

namespace TarimTakip.API.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly ApplicationDbContext _context;

        public QuestionService(ApplicationDbContext context)
        {
            _context = context;
        }

        // HERKESE AÇIK: Tüm soruları listele
        public async Task<List<QuestionResponseDto>> GetAllQuestionsAsync()
        {
            return await _context.Questions
                .Include(q => q.Farmer) // Soruyu soran çiftçinin adını almak için
                .Include(q => q.Answers) // Cevap sayısını almak için
                .OrderByDescending(q => q.CreatedAt)
                .Select(q => new QuestionResponseDto
                {
                    Id = q.Id,
                    Title = q.Title,
                    Status = q.Status,
                    FarmerName = q.Farmer.FullName,
                    ImageUrl = q.ImageUrl,
                    CreatedAt = q.CreatedAt,
                    AnswerCount = q.Answers.Count()
                })
                .ToListAsync();
        }

        // HERKESE AÇIK: Soru detayını ve cevaplarını getir
        public async Task<QuestionDetailDto> GetQuestionDetailAsync(int questionId)
        {
            var question = await _context.Questions
                .Where(q => q.Id == questionId)
                .Include(q => q.Farmer)
                .Include(q => q.Answers) // Cevapları yükle
                    .ThenInclude(a => a.Engineer) // Cevabı yazan mühendisi yükle
                .Select(q => new QuestionDetailDto
                {
                    Id = q.Id,
                    Title = q.Title,
                    Content = q.Content,
                    Status = q.Status,
                    FarmerName = q.Farmer.FullName,
                    ImageUrl = q.ImageUrl,
                    CreatedAt = q.CreatedAt,
                    Answers = q.Answers.Select(a => new AnswerResponseDto
                    {
                        Id = a.Id,
                        Content = a.Content,
                        CreatedAt = a.CreatedAt,
                        EngineerName = a.Engineer.FullName
                    }).OrderBy(a => a.CreatedAt).ToList()
                })
                .FirstOrDefaultAsync();

            if (question == null)
            {
                throw new Exception("Soru bulunamadı.");
            }
            return question;
        }

        // ÇİFTÇİYE ÖZEL: Soru oluştur
        public async Task<int> CreateQuestionAsync(QuestionCreateDto request, int farmerId)
        {
            var question = new Question
            {
                Title = request.Title,
                Content = request.Content,
                FarmerId = farmerId,
                Status = "Pending", // İlk durum 'Beklemede'
                CreatedAt = DateTime.UtcNow,
                ImageUrl = request.ImageUrl
            };

            await _context.Questions.AddAsync(question);
            await _context.SaveChangesAsync();

            return question.Id; // Oluşan sorunun ID'sini döndür
        }

        // MÜHENDİSE ÖZEL: Cevap oluştur
        public async Task<AnswerResponseDto> CreateAnswerAsync(int questionId, AnswerCreateDto request, int engineerId)
        {
            // 1. Cevap yazılacak soru var mı?
            var question = await _context.Questions.FindAsync(questionId);
            if (question == null)
            {
                throw new Exception("Cevap yazmak için soru bulunamadı.");
            }

            // 2. Cevabı oluştur
            var answer = new Answer
            {
                QuestionId = questionId,
                EngineerId = engineerId,
                Content = request.Content,
                CreatedAt = DateTime.UtcNow
            };

            await _context.Answers.AddAsync(answer);

            // 3. Soruya cevap geldiği için statüsünü güncelle
            question.Status = "Answered";
            _context.Questions.Update(question);

            await _context.SaveChangesAsync();

            // 4. Mühendisin adını da içeren DTO'yu döndür
            var engineer = await _context.Users.FindAsync(engineerId);
            return new AnswerResponseDto
            {
                Id = answer.Id,
                Content = answer.Content,
                CreatedAt = answer.CreatedAt,
                EngineerName = engineer?.FullName ?? "Mühendis"
            };
        }

        // ADMIN İÇİN: Soruyu mühendise atama
        public async Task AssignQuestionAsync(int questionId, int engineerId)
        {
            var question = await _context.Questions.FindAsync(questionId);
            if (question == null)
                throw new Exception("Soru bulunamadı.");

            var engineer = await _context.Users.FindAsync(engineerId);
            if (engineer == null)
                throw new Exception("Kullanıcı bulunamadı.");

            // Atanan kişi gerçekten mühendis mi? (Güvenlik kontrolü)
            if (engineer.Role != "Engineer")
                throw new Exception("Bu kullanıcı bir Mühendis değil! Soru atanamaz.");

            question.EngineerId = engineerId;
            question.Status = "Assigned"; // Durumu 'Atandı' yapıyoruz

            _context.Questions.Update(question);
            await _context.SaveChangesAsync();
        }

        // MÜHENDİS İÇİN: Bana atanan soruları getir
        public async Task<List<QuestionResponseDto>> GetQuestionsByEngineerAsync(int engineerId)
        {
            return await _context.Questions
                .Where(q => q.EngineerId == engineerId) // Sadece bu mühendise ait olanlar
                .Include(q => q.Farmer)
                .Include(q => q.Answers)
                .OrderByDescending(q => q.CreatedAt)
                .Select(q => new QuestionResponseDto
                {
                    Id = q.Id,
                    Title = q.Title,
                    Status = q.Status,
                    FarmerName = q.Farmer.FullName,
                    ImageUrl = q.ImageUrl,
                    CreatedAt = q.CreatedAt,
                    AnswerCount = q.Answers.Count()
                })
                .ToListAsync();
        }
    }
}