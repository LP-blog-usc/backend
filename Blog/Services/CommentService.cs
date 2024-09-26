using Blog.Data;
using Blog.Models;
using Blog.Models.Dtos.Request;
using Blog.Models.Dtos.Response;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Enums;
using Blog.Models.DataSet;
using Blog.Services.IServices;

namespace Blog.Services
{
    public class CommentService : ICommentsService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CommentService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Método para crear un nuevo comentario
        public async Task<CommentResponseDto> CreateCommentAsync(CommentRequestDto commentRequestDto)
        {
            // Verificar si el Post existe
            var post = await _context.Posts.FindAsync(commentRequestDto.PostId);
            if (post == null)
            {
                throw new KeyNotFoundException("The specified post does not exist.");
            }

            // Verificar si el Usuario existe
            var user = await _context.Users.FindAsync(commentRequestDto.UserId);
            if (user == null)
            {
                throw new KeyNotFoundException("The specified user does not exist.");
            }

            // Mapeo del DTO al modelo Comment
            var comment = _mapper.Map<Comment>(commentRequestDto);
            comment.CreatedAt = DateTime.UtcNow;

            // Guardar el comentario en la base de datos
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            // Mapear el modelo Comment a CommentResponseDto
            var commentResponse = _mapper.Map<CommentResponseDto>(comment);
            commentResponse.UserName = $"{user.Name} {user.LastName}";

            return commentResponse;
        }

        // Método para obtener todos los comentarios de un post
        public async Task<IEnumerable<CommentResponseDto>> GetCommentsByPostIdAsync(int postId)
        {
            // Obtener los comentarios y sus usuarios asociados
            var comments = await _context.Comments
                .Where(c => c.PostId == postId)
                .ToListAsync();

            // Obtener todos los usuarios relacionados con los comentarios en una sola consulta
            var userIds = comments.Select(c => c.UserId).Distinct().ToList();
            var users = await _context.Users
                .Where(u => userIds.Contains(u.Id))
                .Select(u => new
                {
                    u.Id,
                    UserName = $"{u.Name} {u.LastName}"
                })
                .ToDictionaryAsync(u => u.Id, u => u.UserName);

            // Mapear los comentarios a CommentResponseDto y asignar los nombres de usuario
            var commentResponseDtos = comments.Select(c => new CommentResponseDto
            {
                Content = c.Content,
                CreatedAt = c.CreatedAt,
                UserName = users.TryGetValue(c.UserId, out var userName) ? userName : "Unknown User"
            });

            return commentResponseDtos;
        }



        // Método para eliminar un comentario
        public async Task<bool> DeleteCommentAsync(int commentId, int userId)
        {
            // Buscar el comentario en la base de datos
            var comment = await _context.Comments.FindAsync(commentId);
            if (comment == null)
            {
                throw new KeyNotFoundException("The specified comment does not exist.");
            }

            // Verificar si el usuario es el autor del comentario o un moderador
            var user = await _context.Users.FindAsync(userId);
            if (user == null || (comment.UserId != userId && user.RoleId != (int)UserRoleEnum.Moderador))
            {
                throw new UnauthorizedAccessException("You are not authorized to delete this comment.");
            }

            // Eliminar el comentario
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
