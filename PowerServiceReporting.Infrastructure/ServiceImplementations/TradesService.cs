using PowerServiceReporting.ApplicationCore.Interfaces;
using Services;
using PowerServiceReporting.ApplicationCore.DTOs;
using AutoMapper;
using PowerServiceReporting.ApplicationCore.Helpers;
using Serilog;
using System.Reflection;

namespace PowerServiceReporting.Infrastructure.ServiceImplementations
{
    public class TradesService : ITradesService
    {
        private DateTime clientLocalTime;
        private IMapper mapper;

        public TradesService(IMapper mapper, DateTime clientLocalTime) 
        { 
            this.clientLocalTime = clientLocalTime;
            this.mapper = mapper;
        }

        public async Task<List<PowerTradeDTO>> HandleTrades(CancellationToken stoppingToken)
        {
            List<PowerTradeDTO> powerTradesDTOs = new List<PowerTradeDTO>();
            var powerService = new PowerService();
            try
            {
                var powerTrades = await powerService.GetTradesAsync(clientLocalTime);         
                powerTradesDTOs = mapper.MapList<PowerTrade, PowerTradeDTO>(powerTrades.ToList());
            }
            catch (Exception ex)
            {
                Log.Error($"[{Assembly.GetEntryAssembly().GetName().Name}] => [{typeof(ExportMapperHelper).Name}.{ReflectionHelper.GetActualAsyncMethodName()}]" +
                    $" - failed at Client Local Time {clientLocalTime} with Exception:\n  -Message: {ex.Message}\n  -StackTrace: {ex.StackTrace}");
            }

            return powerTradesDTOs;
        }
    }
}
