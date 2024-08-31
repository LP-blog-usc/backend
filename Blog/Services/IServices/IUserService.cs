using Blog.Models.Dtos.Request;
using Blog.Models.Dtos.Response;
using Blog.Models.Dtos.UpdateDtos;

namespace Blog.Services.IServices
{
    public interface IUserService
    {
        Task<IEnumerable<UserResponseDto>> GetUsersAsync();
        Task<UserResponseDto> GetUserByIdAsync(int id);
        Task<bool> UpdateUserAsync(int id, UserUpdateDto userDto);
        Task<UserResponseDto> CreateUserAsync(UserRequestDto userDto);
        Task<bool> DeleteUserAsync(int id);
        Task<bool> UserExistsByEmailAsync(string email);

    }
}
