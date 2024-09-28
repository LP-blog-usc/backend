using Blog.Models.Dtos.Request;
using Blog.Models.Dtos.Response;
using Blog.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class LikesController : ControllerBase
    {
        private readonly ILikeService _likeService;

        public LikesController(ILikeService likeService)
        {
            _likeService = likeService;
        }

        // Endpoint para crear un like
        [HttpPost]
        public async Task<ActionResult<ApiResponse<string>>> CreateLike(LikeRequestDto likeRequestDto)
        {
            try
            {
                var result = await _likeService.AddLikeAsync(likeRequestDto);

                if (!result)
                {
                    return BadRequest(new ApiResponse<string>
                    {
                        Success = false,
                        Message = "Unable to add like."
                    });
                }

                return Ok(new ApiResponse<string>
                {
                    Success = true,
                    Message = "Like successfully added."
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiResponse<string>
                {
                    Success = false,
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>
                {
                    Success = false,
                    Message = $"An unexpected error occurred: {ex.Message}"
                });
            }
        }

        // Endpoint para eliminar un like
        [HttpDelete]
        public async Task<ActionResult<ApiResponse<string>>> RemoveLike(LikeRequestDto likeRequestDto)
        {
            try
            {
                var result = await _likeService.RemoveLikeAsync(likeRequestDto);

                if (!result)
                {
                    return BadRequest(new ApiResponse<string>
                    {
                        Success = false,
                        Message = "Unable to remove like."
                    });
                }

                return Ok(new ApiResponse<string>
                {
                    Success = true,
                    Message = "Like successfully removed."
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiResponse<string>
                {
                    Success = false,
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>
                {
                    Success = false,
                    Message = $"An unexpected error occurred: {ex.Message}"
                });
            }
        }

        // Endpoint para obtener todos los posts que le gustan a un usuario
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<ApiResponse<List<PostResponseDto>>>> GetLikedPosts(int userId)
        {
            try
            {
                var likedPosts = await _likeService.GetLikedPostsByUserAsync(userId);

                if (likedPosts == null || !likedPosts.Any())
                {
                    return Ok(new ApiResponse<List<PostResponseDto>>
                    {
                        Success = true,
                        Message = "No liked posts found for this user.",
                        Data = new List<PostResponseDto>()
                    });
                }

                return Ok(new ApiResponse<List<PostResponseDto>>
                {
                    Success = true,
                    Message = "Liked posts retrieved successfully.",
                    Data = likedPosts
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiResponse<List<PostResponseDto>>
                {
                    Success = false,
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<List<PostResponseDto>>
                {
                    Success = false,
                    Message = $"An unexpected error occurred: {ex.Message}"
                });
            }
        }
    }

}
