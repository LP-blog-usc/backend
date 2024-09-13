using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Blog.Models;
using Blog.Services.IServices;
using Blog.Models.Dtos.Request;
using Blog.Models.Dtos.Response;

namespace Blog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostsController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<PostResponseDto>>>> GetAllPosts()
        {
            try
            {
                var posts = await _postService.GetAllPostsAsync();

                return Ok(new ApiResponse<IEnumerable<PostResponseDto>>
                {
                    Success = true,
                    Message = "Posts retrieved successfully.",
                    Data = posts
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<IEnumerable<PostResponseDto>>
                {
                    Success = false,
                    Message = "An error occurred while retrieving posts.",
                    Errors = new Dictionary<string, List<string>>
            {
                { "Exception", new List<string> { ex.Message } }
            }
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<PostResponseDto>>> GetPost(int id)
        {
            try
            {
                var post = await _postService.GetPostByIdAsync(id);

                if (post == null)
                {
                    return NotFound(new ApiResponse<PostResponseDto>
                    {
                        Success = false,
                        Message = "Post not found.",
                        Errors = new Dictionary<string, List<string>>
                {
                    { "Id", new List<string> { "No post exists with the provided Id." } }
                }
                    });
                }

                return Ok(new ApiResponse<PostResponseDto>
                {
                    Success = true,
                    Message = "Post retrieved successfully.",
                    Data = post
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<PostResponseDto>
                {
                    Success = false,
                    Message = "An error occurred while retrieving the post.",
                    Errors = new Dictionary<string, List<string>>
            {
                { "Exception", new List<string> { ex.Message } }
            }
                });
            }
        }


        // POST: api/Posts
        [HttpPost]
        public async Task<ActionResult<ApiResponse<PostResponseDto>>> CreatePost(PostRequestDto postDto)
        {
            try
            {
                // Valida si el autor existe
                if (!await _postService.AuthorExistsAsync(postDto.AuthorId))
                {
                    return BadRequest(new ApiResponse<PostResponseDto>
                    {
                        Success = false,
                        Message = "Post creation failed.",
                        Errors = new Dictionary<string, List<string>>
                        {
                            { "AuthorId", new List<string> { "The specified author does not exist." } }
                        }
                    });
                }

                // Crear el post
                var createdPostDto = await _postService.CreatePostAsync(postDto);
                return Ok(new ApiResponse<PostResponseDto>
                {
                    Success = true,
                    Message = "Post created successfully.",
                    Data = createdPostDto
                });
            }
            catch (Exception ex)
            {
                // Manejo general de excepciones
                return StatusCode(500, new ApiResponse<PostResponseDto>
                {
                    Success = false,
                    Message = "An unexpected error occurred while creating the post.",
                    Errors = new Dictionary<string, List<string>>
                    {
                        { "Exception", new List<string> { ex.Message } }
                    }
                });
            }
        }
    }
}
