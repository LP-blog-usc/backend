using AutoMapper;
using Blog.Models.DataSet;
using Blog.Models.Dtos.Request;
using Blog.Models.Dtos.Response;

namespace Blog.Profiles
{
    public class PostProfile : Profile
    {
        public PostProfile()
        {
            // Mapea de PostRequestDto a Post (usado para crear un nuevo post)
            CreateMap<PostRequestDto, Post>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Body, opt => opt.MapFrom(src => src.Body))
                .ForMember(dest => dest.AuthorId, opt => opt.MapFrom(src => src.AuthorId));

            // Mapea de Post a PostResponseDto (usado para devolver la información de un post creado)
            CreateMap<Post, PostResponseDto>()
               .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
               .ForMember(dest => dest.Body, opt => opt.MapFrom(src => src.Body))
               .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments ?? new List<Comment>()))
               .ForMember(dest => dest.Likes, opt => opt.MapFrom(src => src.Likes ?? new List<Like>()))
               .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt));
        }
    }
}
