namespace TarimTakip.API.Models.DTOs.Blog
{
    public class BlogPostResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public string EngineerName { get; set; } // Yazıyı kimin yazdığını göstereceğiz
    }
}