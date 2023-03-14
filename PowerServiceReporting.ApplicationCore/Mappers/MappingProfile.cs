using AutoMapper;
using PowerServiceReporting.ApplicationCore.DTOs;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerServiceReporting.ApplicationCore.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            try
            {
                CreateMap<PowerPeriod, PowerPeriodDTO>();
                CreateMap<PowerTrade, PowerTradeDTO>()
                    .ForMember(dest => dest.Periods, act => act.MapFrom(src => src.Periods));
            }
            catch(Exception ex)
            {
                Console.WriteLine($"{ex.Message} + {ex.StackTrace}");
            }
        }
    }
}
