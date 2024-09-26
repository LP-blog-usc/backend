namespace Blog.Models.Dtos.Response
{
    public class CommentResponseDto
    {
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UserName { get; set; }
    }

}
