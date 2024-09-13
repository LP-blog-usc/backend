using Blog.Models.Dtos.Request;
using Blog.Models.Dtos.Response;
using Blog.Models.Dtos.UpdateDtos;

namespace Blog.Services.IServices
{
    public interface IPostService
    {
        Task<IEnumerable<PostResponseDto>> GetAllPostsAsync();
        Task<PostResponseDto> GetPostByIdAsync(int id);
        //Task<bool> UpdatePostAsync(int id, PostUpdateDto PostDto);
        Task<PostResponseDto> CreatePostAsync(PostRequestDto PostDto);
        //Task<PostResponseDto> DeletePostAsync(int id);
        Task<bool> AuthorExistsAsync(int authorId);
    }
}
