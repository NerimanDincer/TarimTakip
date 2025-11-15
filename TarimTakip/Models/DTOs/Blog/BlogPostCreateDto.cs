using System.ComponentModel.DataAnnotations;

namespace TarimTakip.API.Models.DTOs.Blog
{
    public class BlogPostCreateDto
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }
    }
}