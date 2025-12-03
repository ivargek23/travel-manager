using AutoMapper;

namespace WebAPI.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<BL.BLModels.BLTravel, Dtos.TravelDto>()
                .ForMember(dest => dest.ImageId, opt => opt.MapFrom(src => src.ImageId))
                .ForMember(dest => dest.DestinationId, opt => opt.MapFrom(src => src.DestinationId))
                .ForMember(dest => dest.DestinationName, opt => opt.MapFrom(src => src.DestinationName))
                .ForMember(dest => dest.GuideIds, opt => opt.MapFrom(src => src.GuideIds))
                .ForMember(dest => dest.UserIds, opt => opt.MapFrom(src => src.UserIds));
            CreateMap<BL.BLModels.BLUser, Dtos.UserDto>().ReverseMap();
            CreateMap<BL.BLModels.BLUserLogin, Dtos.UserLoginDto>().ReverseMap();
            CreateMap<BL.BLModels.BLDestination, Dtos.DestinationDto>().ReverseMap();
            CreateMap<BL.BLModels.BLGuide, Dtos.GuideDto>().ReverseMap();
            CreateMap<BL.BLModels.BLSearch, Dtos.SearchDto>().ReverseMap();
        }
    }
}
