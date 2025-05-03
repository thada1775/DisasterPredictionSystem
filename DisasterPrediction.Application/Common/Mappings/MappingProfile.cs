using AutoMapper;
using DisasterPrediction.Application.DTOs;
using DisasterPrediction.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisasterPrediction.Application.Common.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Region, RegionDto>()
                .ForMember(dest => dest.DisasterTypes, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(dest => dest.DisasterTypes, opt => opt.Ignore());
            CreateMap<LocationCoordinates, LocationCoordinatesDto>().ReverseMap();
            CreateMap<AlertSetting, AlertSettingDto>().ReverseMap();
            CreateMap<AlertHistory, AlertHistoryDto>().ReverseMap();
        }
    }
}
