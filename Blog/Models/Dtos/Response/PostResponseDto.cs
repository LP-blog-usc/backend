using Blog.Models.DataSet;
using System.ComponentModel.DataAnnotations;

namespace Blog.Models.Dtos.Response
{
    public class PostResponseDto
    {
        public string Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public DateTime UpdatedAt { get; set; }
        public string AuthorName { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<Like>? Likes { get; set; }
    }
}
