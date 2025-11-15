using TarimTakip.API.Data;
using TarimTakip.API.Data.Entities;
using TarimTakip.API.Models.DTOs.Farm;

namespace TarimTakip.API.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly ApplicationDbContext _context;

        public ExpenseService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateExpenseAsync(int farmFieldId, ExpenseCreateDto request, int userId)
        {
            // 1. GÜVENLİK KONTROLÜ: Önce tarla var mı ve bu kullanıcıya mı ait?
            var farmField = await _context.FarmFields
                .FindAsync(farmFieldId);

            if (farmField == null)
            {
                throw new Exception("Tarla bulunamadı."); // Veya "NotFoundException"
            }

            if (farmField.UserId != userId)
            {
                // Tarla bulundu AMA bu kullanıcıya ait değil!
                throw new Exception("Bu tarlaya masraf ekleme yetkiniz yok."); // Veya "UnauthorizedAccessException"
            }

            // 2. Kontroller başarılıysa, masrafı oluştur ve ekle
            var expense = new Expense
            {
                FarmFieldId = farmFieldId, // Kontrol ettiğimiz tarlanın ID'si
                CostType = request.CostType,
                Amount = request.Amount,
                Date = request.Date,
                Note = request.Note
            };

            await _context.Expenses.AddAsync(expense);
            await _context.SaveChangesAsync();
        }
    }
}