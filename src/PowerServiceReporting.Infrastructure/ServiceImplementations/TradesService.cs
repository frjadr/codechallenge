using PowerServiceReporting.ApplicationCore.Interfaces;
using Services;
using PowerServiceReporting.ApplicationCore.DTOs;
using AutoMapper;
using PowerServiceReporting.ApplicationCore.Helpers;
using Serilog;
using System.Reflection;

namespace PowerServiceReporting.Infrastructure.ServiceImplementations
{
    /// <summary>
    /// Class for trades fetching and mapping.
    /// </summary>
    public class TradesService : ITradesService
    {
        private readonly DateTime _clientLocalTime;
        private readonly IMapper _mapper;

        public TradesService(IMapper mapper, DateTime clientLocalTime) 
        { 
            _clientLocalTime = clientLocalTime;
            _mapper = mapper;
        }

        /// <summary>
        /// Handles randomly generated trade data from PowerService and maps them to DTO.
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        public async Task<List<PowerTradeDTO>> HandleTrades(CancellationToken stoppingToken)
        {
            List<PowerTradeDTO> powerTradesDTOs = new List<PowerTradeDTO>();
            var powerService = new PowerService();
            try
            {
                var powerTrades = await powerService.GetTradesAsync(_clientLocalTime);         
                powerTradesDTOs = _mapper.MapList<PowerTrade, PowerTradeDTO>(powerTrades.ToList());
            }
            catch (Exception ex)
            {
                Log.Error($"[{Assembly.GetEntryAssembly().GetName().Name}] => [{typeof(ExportMapperHelper).Name}.{ReflectionHelper.GetActualAsyncMethodName()}]" +
                    $" - failed at Client Local Time {_clientLocalTime} with Exception:\n  -Message: {ex.Message}\n  -StackTrace: {ex.StackTrace}");
            }

            return powerTradesDTOs;
        }
    }
}
