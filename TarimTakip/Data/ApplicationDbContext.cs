using Microsoft.EntityFrameworkCore;
using TarimTakip.API.Data.Entities;

namespace TarimTakip.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        // Constructor
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Entity'lerimizi (tablolarımızı) buraya ekliyoruz
        public DbSet<User> Users { get; set; }
        public DbSet<EngineerProfile> EngineerProfiles { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<FarmField> FarmFields { get; set; }
        public DbSet<PlantInfo> PlantInfos { get; set; }
        public DbSet<PlantRegion> PlantRegions { get; set; }
        public DbSet<Fertilization> Fertilizations { get; set; }
        public DbSet<Irrigation> Irrigations { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<CalendarNote> CalendarNotes { get; set; }
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<ChatRoom> ChatRooms { get; set; }
        public DbSet<Message> Messages { get; set; }

        // İlişkileri detaylandırmak için (opsiyonel ama güçlü)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // EngineerProfile ve User arasında 1'e 1 ilişki (Bu doğru)
            modelBuilder.Entity<EngineerProfile>()
                .HasOne(ep => ep.User)
                .WithOne(u => u.EngineerProfile)
                .HasForeignKey<EngineerProfile>(ep => ep.UserId)
                .OnDelete(DeleteBehavior.Cascade); // User silinirse, profili silinsin.

            // --- ChatRoom İlişkileri (DÜZELTME BURADA) ---
            modelBuilder.Entity<ChatRoom>()
                .HasOne(c => c.Farmer)
                .WithMany(u => u.ChatRoomsAsFarmer)
                .HasForeignKey(c => c.FarmerId)
                .OnDelete(DeleteBehavior.Restrict); // <-- HATA ALMAMAK İÇİN KISITLA

            modelBuilder.Entity<ChatRoom>()
                .HasOne(c => c.Engineer)
                .WithMany(u => u.ChatRoomsAsEngineer)
                .HasForeignKey(c => c.EngineerId)
                .OnDelete(DeleteBehavior.Restrict); // <-- HATA ALMAMAK İÇİN KISITLA

            // --- Diğer İlişkiler ---
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany(u => u.SentMessages)
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Cascade); // Mesajlar silinebilir, döngü yok.

            modelBuilder.Entity<BlogPost>()
                .HasOne(b => b.Engineer)
                .WithMany(u => u.BlogPosts)
                .HasForeignKey(b => b.EngineerId)
                .OnDelete(DeleteBehavior.Cascade); // Yazılar silinebilir, döngü yok.

            modelBuilder.Entity<Question>()
                .HasOne(q => q.Farmer)
                .WithMany(u => u.Questions)
                .HasForeignKey(q => q.FarmerId)
                .OnDelete(DeleteBehavior.Cascade); // Sorular silinebilir, döngü yok.

            // --- Answer İlişkisi (DÜZELTME BURADA) ---
            // Bu da 'ChatRoom' gibi çoklu yola neden olabilirdi.
            modelBuilder.Entity<Answer>()
                .HasOne(a => a.Engineer)
                .WithMany(u => u.Answers)
                .HasForeignKey(a => a.EngineerId)
                .OnDelete(DeleteBehavior.Restrict); // <-- HATA ALMAMAK İÇİN KISITLA

            // --- Bölge Kuralları (Bunlar KALSIN, BÖLGE SİLMEK İÇİN) ---
            modelBuilder.Entity<User>()
                .HasOne(u => u.Region)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RegionId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FarmField>()
                .HasOne(f => f.Region)
                .WithMany(r => r.FarmFields)
                .HasForeignKey(f => f.RegionId)
                .OnDelete(DeleteBehavior.Restrict);

            // --- FarmField -> User (Bu KALSIN) ---
            modelBuilder.Entity<FarmField>()
                .HasOne(f => f.User)
                .WithMany(u => u.FarmFields)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // --- CalendarNote (Bu KALSIN) ---
            modelBuilder.Entity<CalendarNote>()
                .HasOne(cn => cn.User)
                .WithMany(u => u.CalendarNotes)
                .HasForeignKey(cn => cn.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}