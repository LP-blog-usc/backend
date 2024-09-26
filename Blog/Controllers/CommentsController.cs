using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Blog.Data;
using Blog.Models.DataSet;
using Blog.Models.Dtos.Request;
using Blog.Models.Dtos.Response;
using Blog.Services.IServices;

namespace Blog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentsService _commentsService;

        public CommentsController(ICommentsService commentsService)
        {
            _commentsService = commentsService;
        }

        // POST /api/comments
        [HttpPost]
        public async Task<ActionResult<ApiResponse<CommentResponseDto>>> CreateComment(CommentRequestDto commentRequestDto)
        {
            try
            {
                var createdComment = await _commentsService.CreateCommentAsync(commentRequestDto);

                return Ok(new ApiResponse<CommentResponseDto>
                {
                    Success = true,
                    Message = "Comment successfully created.",
                    Data = createdComment
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiResponse<CommentResponseDto>
                {
                    Success = false,
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<CommentResponseDto>
                {
                    Success = false,
                    Message = $"An unexpected error occurred: {ex.Message}"
                });
            }
        }

        [HttpGet("posts/{postId}/comments")]
        public async Task<IActionResult> GetCommentsByPostId(int postId)
        {
            try
            {
                // Llama al servicio para obtener los comentarios del post
                var comments = await _commentsService.GetCommentsByPostIdAsync(postId);

                return Ok(new ApiResponse<IEnumerable<CommentResponseDto>>
                {
                    Success = true,
                    Message = "Comments retrieved successfully.",
                    Data = comments
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiResponse<IEnumerable<CommentResponseDto>>
                {
                    Success = false,
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<IEnumerable<CommentResponseDto>>
                {
                    Success = false,
                    Message = $"An unexpected error occurred: {ex.Message}"
                });
            }
        }
    }
}
