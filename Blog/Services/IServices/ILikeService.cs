using Blog.Models.Dtos.Request;
using Blog.Models.Dtos.Response;

namespace Blog.Services.IServices
{
    public interface ILikeService
    {
        Task<Boolean> AddLikeAsync(LikeRequestDto likeRequestDto);
        Task<Boolean> RemoveLikeAsync(LikeRequestDto unlikeRequestDto);
        Task<List<PostResponseDto>> GetLikedPostsByUserAsync(int userId);

    }
}
