namespace TarimTakip.API.Data.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string? Phone { get; set; }
        public string Role { get; set; } // "Farmer" veya "Engineer"
        public int RegionId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // --- Navigation Properties (İlişkiler) ---

        // 1 User -> 1 Region
        public virtual Region Region { get; set; }

        // 1 User -> 1 EngineerProfile (Opsiyonel)
        public virtual EngineerProfile? EngineerProfile { get; set; }

        // 1 User -> Many FarmFields
        public virtual ICollection<FarmField> FarmFields { get; set; }

        // 1 User -> Many CalendarNotes
        public virtual ICollection<CalendarNote> CalendarNotes { get; set; }

        // 1 User (Farmer) -> Many Questions
        public virtual ICollection<Question> Questions { get; set; }

        // 1 User (Engineer) -> Many Answers
        public virtual ICollection<Answer> Answers { get; set; }

        // 1 User (Engineer) -> Many BlogPosts
        public virtual ICollection<BlogPost> BlogPosts { get; set; }

        // 1 User -> Many SentMessages
        public virtual ICollection<Message> SentMessages { get; set; }

        // 1 User (Farmer) -> Many ChatRooms
        public virtual ICollection<ChatRoom> ChatRoomsAsFarmer { get; set; }

        // 1 User (Engineer) -> Many ChatRooms
        public virtual ICollection<ChatRoom> ChatRoomsAsEngineer { get; set; }
    }
}