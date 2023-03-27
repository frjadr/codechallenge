using AutoMapper;
using PowerServiceReporting.ApplicationCore.DTOs;
using PowerServiceReporting.ApplicationCore.Helpers;
using Serilog;
using Services;
using System.Reflection;

namespace PowerServiceReporting.ApplicationCore.Mappers
{
    /// <summary>
    /// AutoMapper class used for creating maps, matching classes and members for mappings.
    /// </summary>
    public class MappingProfile : Profile
    {
        public MappingProfile(DateTime clientLocalTime)
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
                $" - failed at Client Local Time {clientLocalTime} with Exception:\n  -Message: {ex.Message}\n  -StackTrace: {ex.StackTrace}");
            }
        }
    }
}
