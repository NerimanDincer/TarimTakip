using Microsoft.EntityFrameworkCore;
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

        // BU KISMI MEVCUT KODUNUN ALTINA YAPIŞTIR

        // LİSTELEME (SİLİNENLERİ GETİRME)
        public async Task<List<Expense>> GetExpensesByFieldAsync(int farmFieldId, int userId)
        {
            // Önce tarlanın sahibini kontrol etmemiz lazım ama 
            // pratik olsun diye sorgunun içine gömüyoruz:

            var expenses = await _context.Expenses
                .Include(x => x.FarmField) // Tarlaya eriş
                .Where(x => x.FarmFieldId == farmFieldId
                            && x.FarmField.UserId == userId // Sadece kendi tarlası
                            && !x.IsDeleted) // <--- İŞTE KRİTİK NOKTA: SİLİNMİŞSE GETİRME!
                .OrderByDescending(x => x.Date) // En yeniler üstte
                .ToListAsync();

            return expenses;
        }

        // SİLME (SOFT DELETE - GİZLEME)
        public async Task DeleteExpenseAsync(int expenseId, int userId)
        {
            var expense = await _context.Expenses
                .Include(x => x.FarmField)
                .FirstOrDefaultAsync(x => x.Id == expenseId);

            if (expense == null) throw new Exception("Masraf bulunamadı.");

            // Başkasının masrafını silemesin
            if (expense.FarmField.UserId != userId)
                throw new Exception("Bunu silmeye yetkiniz yok.");

            // HARD DELETE (ESKİ): _context.Expenses.Remove(expense);

            // SOFT DELETE (YENİ):
            expense.IsDeleted = true; // Sadece bayrağı kaldır!

            await _context.SaveChangesAsync();
        }
    }
}