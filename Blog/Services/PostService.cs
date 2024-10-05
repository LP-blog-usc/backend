using AutoMapper;
using Blog.Data;
using Blog.Enums;
using Blog.Models.DataSet;
using Blog.Models.Dtos.Request;
using Blog.Models.Dtos.Response;
using Blog.Models.Dtos.UpdateDtos;
using Blog.Models.InnerModels;
using Blog.Services.IServices;
using Microsoft.EntityFrameworkCore;
using NuGet.Configuration;

namespace Blog.Services
{
    public class PostService : IPostService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public PostService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PostResponseDto>> GetAllPostsAsync()
        {
            var postsWithAuthors = await _context.Posts
                .Include(p => p.Comments)
                .Include(p => p.Likes)
                .Select(p => new PostWithAuthor
                {
                    Post = p,
                    AuthorName = _context.Users
                        .Where(u => u.Id == p.AuthorId)
                        .Select(u => u.Name + " " + u.LastName)
                        .FirstOrDefault() ?? "Unknown Author"
                })
                .ToListAsync();

            return _mapper.Map<IEnumerable<PostResponseDto>>(postsWithAuthors);
        }

        public async Task<List<PostResponseDto>> GetPostsByAuthorAsync(int authorId)
        {
            // Obtiene todos los posts asociados al autor
            var postsWithAuthor = await _context.Posts
                .Where(p => p.AuthorId == authorId)
                .Include(p => p.Comments)
                .Include(p => p.Likes)
                .Select(p => new PostWithAuthor
                {
                    Post = p,
                    AuthorName = _context.Users
                        .Where(u => u.Id == p.AuthorId)
                        .Select(u => u.Name + " " + u.LastName)
                        .FirstOrDefault() ?? "Unknown Author"
                })
                .ToListAsync();

            // Mapea los resultados a PostResponseDto
            var postResponseDtos = _mapper.Map<List<PostResponseDto>>(postsWithAuthor);

            return postResponseDtos;
        }


        // Método para obtener un post por Id
        public async Task<PostResponseDto?> GetPostByIdAsync(int id)
        {
            // Obtiene un post por su Id incluyendo los comentarios, likes y el nombre del autor
            var postWithAuthor = await _context.Posts
                .Include(p => p.Comments)
                    .ThenInclude(c => c.User) // Incluye el usuario de cada comentario
                .Include(p => p.Likes)
                    .ThenInclude(l => l.User) // Incluye el usuario de cada like
                .Where(p => p.Id == id)
                .Select(p => new PostWithAuthor
                {
                    Post = p,
                    AuthorName = p.Author != null ? p.Author.Name + " " + p.Author.LastName : "Unknown Author"
                })
                .FirstOrDefaultAsync();

            // Si el post no existe, devuelve null
            if (postWithAuthor == null)
            {
                return null;
            }

            // Mapea el PostWithAuthor a PostResponseDto y lo devuelve
            return _mapper.Map<PostResponseDto>(postWithAuthor);
        }



        // Verifica si el autor existe
        public async Task<bool> AuthorExistsAsync(int authorId)
        {
            return await _context.Users.AnyAsync(u => u.Id == authorId && u.RoleId == (int)UserRoleEnum.Autor);
        }

        // Crea un nuevo post
        public async Task<PostResponseDto> CreatePostAsync(PostRequestDto postDto)
        {
            // Mapea el PostRequestDto a la entidad Post usando AutoMapper
            var post = _mapper.Map<Post>(postDto);

            // Agrega fechas de creación y actualización
            post.CreatedAt = DateTime.UtcNow;
            post.UpdatedAt = DateTime.UtcNow;

            // Agregar el post al contexto
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            // Obtener el autor (nombre y apellido) por el AuthorId
            var authorName = await _context.Users
                .Where(u => u.Id == postDto.AuthorId)
                .Select(u => u.Name + " " + u.LastName)
                .FirstOrDefaultAsync() ?? "Unknown Author";

            // Crear un PostWithAuthor
            var postWithAuthor = new PostWithAuthor
            {
                Post = post,
                AuthorName = authorName
            };

            // Mapear PostWithAuthor a PostResponseDto
            return _mapper.Map<PostResponseDto>(postWithAuthor);
        }

        public async Task<bool> IsAuthorOfPostAsync(int postId, int authorId)
        {
            // Verifica si el post con el ID especificado pertenece al autor con el ID especificado
            return await _context.Posts.AnyAsync(p => p.Id == postId && p.AuthorId == authorId);
        }

        public async Task<PostResponseDto> UpdatePostAsync(int postId, PostUpdateDto postUpdateDto)
        {
            // Verifica si el post pertenece al autor antes de actualizar
            if (!await IsAuthorOfPostAsync(postId, postUpdateDto.AuthorId))
            {
                throw new UnauthorizedAccessException("You are not authorized to edit this post.");
            }

            // Busca el post existente incluyendo comentarios y likes
            var post = await _context.Posts
                .Include(p => p.Comments)
                .Include(p => p.Likes)
                .FirstOrDefaultAsync(p => p.Id == postId);

            if (post == null)
            {
                throw new KeyNotFoundException("Post not found.");
            }

            // Actualiza los campos si están presentes en el DTO
            if (!string.IsNullOrEmpty(postUpdateDto.Title))
            {
                post.Title = postUpdateDto.Title;
            }

            if (!string.IsNullOrEmpty(postUpdateDto.Body))
            {
                post.Body = postUpdateDto.Body;
            }

            post.UpdatedAt = DateTime.UtcNow;

            // Guarda los cambios en la base de datos
            _context.Posts.Update(post);
            await _context.SaveChangesAsync();

            // Obtener el nombre del autor
            var authorName = await _context.Users
                .Where(u => u.Id == post.AuthorId)
                .Select(u => u.Name + " " + u.LastName)
                .FirstOrDefaultAsync();

            // Crea el objeto intermedio PostWithAuthor
            var postWithAuthor = new PostWithAuthor
            {
                Post = post,
                AuthorName = authorName ?? "Unknown Author"
            };

            // Mapear el PostWithAuthor a PostResponseDto usando AutoMapper
            var postResponseDto = _mapper.Map<PostResponseDto>(postWithAuthor);

            return postResponseDto;
        }

        public async Task<bool> DeletePostAsync(int postId, int authorId)
        {
            // Busca el post por su Id
            var post = await _context.Posts
                .Include(p => p.Comments)  // Incluye los comentarios para verificar si tiene alguno
                .FirstOrDefaultAsync(p => p.Id == postId);

            if (post == null)
            {
                throw new KeyNotFoundException("Post not found.");
            }

            // Verifica si el autor que solicita la eliminación es el dueño del post
            if (post.AuthorId != authorId)
            {
                throw new UnauthorizedAccessException("You are not authorized to delete this post.");
            }

            // Verifica si el post tiene comentarios
            if (post.Comments != null && post.Comments.Any())
            {
                throw new InvalidOperationException("Cannot delete a post that contains comments.");
            }

            // Elimina el post si pasa todas las validaciones
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

            return true; // Retorna true si la eliminación fue exitosa
        }
    }
}
