using AutoMapper;
using Blog.Data;
using Blog.Models.DataSet;
using Blog.Models.Dtos.Request;
using Blog.Models.Dtos.Response;
using Blog.Services.IServices;
using Microsoft.EntityFrameworkCore;

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
            var author = await _context.Users
                .Where(u => u.Id == postDto.AuthorId)
                .Select(u => new { u.Name, u.LastName })
                .FirstOrDefaultAsync();

            // Actualizar el nombre del autor en el Post antes de mapearlo a PostResponseDto
            var postResponse = _mapper.Map<PostResponseDto>(post);
            postResponse.AuthorName = author != null
                ? $"{author.Name} {author.LastName}"
                : "Unknown";

            return postResponse;
        }

    }
}
