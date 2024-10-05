using AutoMapper;
using Blog.Models.DataSet;
using Blog.Models.Dtos.Request;
using Blog.Models.Dtos.Response;

namespace Blog.Profiles
{
    public class LikeProfile : Profile
    {
        public LikeProfile()
        {
            // Mapeo de LikeRequestDto a Like
            CreateMap<LikeRequestDto, Like>()
                .ForMember(dest => dest.PostId, opt => opt.MapFrom(src => src.PostId))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId));

            // Mapeo de Like a LikeResponseDto
            CreateMap<Like, LikeResponseDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Name + " " + src.User.LastName));
        }
    }
}
