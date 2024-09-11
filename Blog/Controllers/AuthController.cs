using Blog.Models.Dtos.Request;
using Blog.Models.Dtos.Response;
using Blog.Services.IServices;
using Microsoft.AspNetCore.Mvc;


namespace Blog.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<LoginResponseDto>>> Login(LoginRequestDto loginDto)
        {
            var result = await _authService.LoginAsync(loginDto.Email, loginDto.Password);

            if (result != null)
            {
                return Ok(new ApiResponse<LoginResponseDto>
                {
                    Success = true,
                    Message = "Login successful",
                    Data = result
                });
            }

            return Unauthorized(new ApiResponse<LoginResponseDto>
            {
                Success = false,
                Message = "Invalid email or password",
                Errors = new Dictionary<string, List<string>>
        {
            { "Auth", new List<string> { "Invalid credentials" } }
        }
            });
        }
    }
}