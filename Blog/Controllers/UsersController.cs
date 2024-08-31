using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Blog.Models;
using Blog.Services;
using Blog.Services.IServices;
using Microsoft.EntityFrameworkCore;
using Blog.Models.Dtos.Request;
using Blog.Models.Dtos.Response;
using Blog.Models.Dtos.UpdateDtos;

namespace Blog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<UserResponseDto>>>> GetUsers()
        {
            var users = await _userService.GetUsersAsync();
            return Ok(new ApiResponse<IEnumerable<UserResponseDto>>
            {
                Success = true,
                Message = "Users retrieved successfully.",
                Data = users
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<UserResponseDto>>> GetUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound(new ApiResponse<UserResponseDto>
                {
                    Success = false,
                    Message = "User not found.",
                    Errors = new Dictionary<string, List<string>>
            {
                { "Id", new List<string> { $"No user found with ID {id}." } }
            }
                });
            }
            return Ok(new ApiResponse<UserResponseDto>
            {
                Success = true,
                Message = "User retrieved successfully.",
                Data = user
            });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> PutUser(int id, UserUpdateDto userDto)
        {
            var userUpdated = await _userService.UpdateUserAsync(id, userDto);
            if (!userUpdated)
            {
                return NotFound(new ApiResponse<bool>
                {
                    Success = false,
                    Message = "User update failed.",
                    Data = false,
                    Errors = new Dictionary<string, List<string>>
            {
                { "Id", new List<string> { $"No user found with ID {id}." } }
            }
                });
            }
            return Ok(new ApiResponse<bool>
            {
                Success = true,
                Message = "User updated successfully.",
                Data = true
            });
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<ApiResponse<UserResponseDto>>> PostUser(UserRequestDto userDto)
        {
            if (await _userService.UserExistsByEmailAsync(userDto.Email))
            {
                return BadRequest(new ApiResponse<UserResponseDto>
                {
                    Success = false,
                    Message = "User creation failed.",
                    Errors = new Dictionary<string, List<string>>
            {
                { "Email", new List<string> { "A user with the provided email already exists." } }
            }
                });
            }

            var createdUserDto = await _userService.CreateUserAsync(userDto);
            return Ok(new ApiResponse<UserResponseDto>
            {
                Success = true,
                Message = "User created successfully.",
                Data = createdUserDto
            });
        }



        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<UserResponseDto>>> DeleteUser(int id)
        {
            var user = await _userService.DeleteUserAsync(id);
            if (user == null)
            {
                return NotFound(new ApiResponse<UserResponseDto>
                {
                    Success = false,
                    Message = "User not found.",
                    Errors = new Dictionary<string, List<string>>
            {
                { "Id", new List<string> { $"No user found with ID {id}." } }
            }
                });
            }
            return Ok(new ApiResponse<UserResponseDto>
            {
                Success = true,
                Message = "User deleted successfully.",
                Data = user
            });
        }
    }
}
