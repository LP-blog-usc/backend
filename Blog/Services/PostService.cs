using AutoMapper;
using Blog.Data;
using Blog.Models.DataSet;
using Blog.Models.Dtos.Request;
using Blog.Models.Dtos.Response;
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

        // Método para obtener un post por Id
        public async Task<PostResponseDto?> GetPostByIdAsync(int id)
        {
            // Obtiene un post por su Id incluyendo los comentarios, likes y el nombre del autor
            var postWithAuthor = await _context.Posts
                .Include(p => p.Comments)
                .Include(p => p.Likes)
                .Where(p => p.Id == id)
                .Select(p => new PostWithAuthor
                {
                    Post = p,
                    AuthorName = _context.Users
                        .Where(u => u.Id == p.AuthorId)
                        .Select(u => u.Name + " " + u.LastName)
                        .FirstOrDefault() ?? "Unknown Author"
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
            return await _context.Users.AnyAsync(u => u.Id == authorId);
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
    }
}
