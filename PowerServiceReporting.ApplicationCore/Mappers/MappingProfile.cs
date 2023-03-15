using AutoMapper;
using PowerServiceReporting.ApplicationCore.DTOs;
using PowerServiceReporting.ApplicationCore.Helpers;
using Serilog;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
                Log.Fatal($"[{Assembly.GetEntryAssembly().GetName().Name}] => [{this.GetType().Name}.{ReflectionHelper.GetActualAsyncMethodName()}]" +
                $" - failed at {DateTime.Now} with Exception:\n  -Message: {ex.Message}\n  -StackTrace: {ex.StackTrace}");
            }
        }
    }
}
