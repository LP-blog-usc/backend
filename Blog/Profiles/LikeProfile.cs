using AutoMapper;
using Blog.Models.DataSet;
using Blog.Models.Dtos.Request;

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
        }
    }

}
