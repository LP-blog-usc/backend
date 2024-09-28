using AutoMapper;
using Blog.Data;
using Blog.Models.DataSet;
using Blog.Models.Dtos.Request;
using Blog.Models.Dtos.Response;
using Blog.Models.InnerModels;
using Blog.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace Blog.Services
{
    public class LikeService : ILikeService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public LikeService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Boolean> AddLikeAsync(LikeRequestDto likeRequestDto)
        {
            // Verificar si el like ya existe para evitar duplicados
            var existingLike = await _context.Likes
                .FirstOrDefaultAsync(l => l.PostId == likeRequestDto.PostId && l.UserId == likeRequestDto.UserId);

            if (existingLike != null)
            {
                throw new InvalidOperationException("The post is already liked by this user.");
            }

            // Crear el nuevo like mapeado desde el DTO
            var like = _mapper.Map<Like>(likeRequestDto);

            // Agregar el like al contexto y guardar cambios
            _context.Likes.Add(like);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<Boolean> RemoveLikeAsync(LikeRequestDto likeRequestDto)
        {
            // Buscar el like que se desea eliminar
            var existingLike = await _context.Likes
                .FirstOrDefaultAsync(l => l.PostId == likeRequestDto.PostId && l.UserId == likeRequestDto.UserId);

            if (existingLike == null)
            {
                throw new InvalidOperationException("The like does not exist.");
            }

            // Eliminar el like del contexto y guardar cambios
            _context.Likes.Remove(existingLike);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<PostResponseDto>> GetLikedPostsByUserAsync(int userId)
        {
            // Verificar si el usuario existe
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {userId} not found.");
            }

            // Obtener los posts que le gustan al usuario con la información del autor
            var likedPostsWithAuthors = await _context.Likes
                .Where(l => l.UserId == userId)
                .Select(l => new PostWithAuthor
                {
                    Post = l.Post,
                    AuthorName = _context.Users
                        .Where(u => u.Id == l.Post.AuthorId)
                        .Select(u => u.Name + " " + u.LastName)
                        .FirstOrDefault() ?? "Unknown Author"
                })
                .ToListAsync();

            // Mapear los PostWithAuthor a PostResponseDto
            var postResponseDtos = _mapper.Map<List<PostResponseDto>>(likedPostsWithAuthors);

            return postResponseDtos;
        }
    }

}
