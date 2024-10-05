using Blog.Models.DataSet;
using Blog.Models.Dtos.Response;
using System.ComponentModel.DataAnnotations;

namespace Blog.Models.Dtos.Response
{
    public class PostResponseDto
    {
        public string Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public DateTime UpdatedAt { get; set; }
        public string AuthorName { get; set; }
        public ICollection<CommentResponseDto>? Comments { get; set; }
        public ICollection<LikeResponseDto>? Likes { get; set; }
    }
}