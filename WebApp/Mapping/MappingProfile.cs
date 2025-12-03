using AutoMapper;
using WebApp.ViewModels;

namespace WebApp.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<BL.BLModels.BLTravel, TravelVM>()
                .ForMember(dest => dest.ImageId, opt => opt.MapFrom(src => src.ImageId))
                .ForMember(dest => dest.DestinationId, opt => opt.MapFrom(src => src.DestinationId))
                .ForMember(dest => dest.DestinationName, opt => opt.MapFrom(src => src.DestinationName))
                .ReverseMap();
            CreateMap<BL.BLModels.BLUser, UserVM>().ReverseMap();
            CreateMap<BL.BLModels.BLUserLogin, UserLoginVM>().ReverseMap();
            CreateMap<BL.BLModels.BLDestination, DestinationVM>().ReverseMap();
            CreateMap<BL.BLModels.BLSearch, SearchVM>().ReverseMap();
            CreateMap<BL.BLModels.BLGuide, GuideVM>().ReverseMap();
            CreateMap<BL.BLModels.BLUserTravel, UserTravelVM>().ReverseMap();
        }
    }
}
