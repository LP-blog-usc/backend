using Blog.Models.Dtos.Response;

namespace Blog.Services.IServices
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(string email, string password);
    }
}
