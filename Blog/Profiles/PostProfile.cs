using AutoMapper;
using Blog.Models.DataSet;
using Blog.Models.Dtos.Request;
using Blog.Models.Dtos.Response;
using Blog.Models.InnerModels;

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

            // Mapea de PostWithAuthor a PostResponseDto
            CreateMap<PostWithAuthor, PostResponseDto>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Post.Title))
                .ForMember(dest => dest.Body, opt => opt.MapFrom(src => src.Post.Body))
                .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.AuthorName))
                .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Post.Comments ?? new List<Comment>()))
                .ForMember(dest => dest.Likes, opt => opt.MapFrom(src => src.Post.Likes ?? new List<Like>()))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.Post.UpdatedAt));
        }
    }
}
