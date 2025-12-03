using AutoMapper;
using BL.BLModels;
using BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Travel, BLTravel>()
                .ForMember(dest => dest.ImageId, opt => opt.MapFrom(src => src.ImageId))
                .ForMember(dest => dest.DestinationId, opt => opt.MapFrom(src => src.DestinationId))
                .ForMember(dest => dest.DestinationName, opt => opt.MapFrom(src => src.Destination.Name))
                .ForMember(dest => dest.GuideIds, opt => opt.MapFrom(src => src.TravelGuides.Select(x => x.GuideId)))
                .ForMember(dest => dest.UserIds, opt => opt.MapFrom(src => src.UserTravels.Select(x => x.UserId)))
                .ReverseMap()
                .ForMember(dest => dest.Destination, opt => opt.Ignore())
                .ForMember(dest => dest.Image, opt => opt.Ignore());

            CreateMap<Travel, Guide>()
                .ForMember(dest => dest.TravelGuides, opt => opt.MapFrom(src => src.TravelGuides.Select(x => x.GuideId)));
            CreateMap<Destination, BLDestination>().ReverseMap();
            CreateMap<Guide, BLGuide>().ReverseMap();
            CreateMap<User, BLUser>()
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId))
                .ReverseMap();
            CreateMap<User, BLUserLogin>().ReverseMap();
            CreateMap<Image, BLImage>().ReverseMap();
            CreateMap<UserTravel, BLUserTravel>().ReverseMap();
        }
    }
}
