using AutoMapper;
using Blog.Models.DataSet;
using Blog.Models.Dtos.Request;
using Blog.Models.Dtos.Response;
using Blog.Models.Dtos.UpdateDtos;

namespace Blog.Profiles
{
    public class CommentProfile : Profile
    {
        public CommentProfile()
        {
            // Mapeo de CommentRequestDto a Comment
            CreateMap<CommentRequestDto, Comment>()
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
                .ForMember(dest => dest.PostId, opt => opt.MapFrom(src => src.PostId))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore()) 
                .ForMember(dest => dest.Post, opt => opt.Ignore())        
                .ForMember(dest => dest.User, opt => opt.Ignore());       
            
            // Mapeo de Comment a CommentResponseDto
            CreateMap<Comment, CommentResponseDto>()
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Name));

            // Mapeo de CommentUpdateDto a Comment (solo actualiza el contenido)
            CreateMap<CommentUpdateDto, Comment>()
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content));
        }
    }
}
