using Blog.Models.Dtos.Request;
using Blog.Models.Dtos.Response;

namespace Blog.Services.IServices
{
    public interface ICommentsService
    {

        Task<CommentResponseDto> CreateCommentAsync(CommentRequestDto commentRequestDto);
        Task<IEnumerable<CommentResponseDto>> GetCommentsByPostIdAsync(int postId);
        Task<bool> DeleteCommentAsync(int commentId, int userId);
    }
}
