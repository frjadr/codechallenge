using PowerServiceReporting.ApplicationCore.Helpers;
using PowerServiceReporting.ApplicationCore.Interfaces;
using Serilog;
using System.Reflection;

namespace PowerServiceReporting.Infrastructure.ServiceImplementations
{
    /// <summary>
    /// Top lvl business logic class used by Worker Service.
    /// </summary>
    public class TradesReportingService : ITradesReportingService
    {
        private readonly ITradesService _tradesHandlerService;
        private readonly IReportExportingService _reportExportingService;
        private readonly DateTime _clientLocalTime;

        public TradesReportingService(ITradesService tradesHandlerService, IReportExportingService reportExportingService, DateTime clientLocalTime)
        {
            _tradesHandlerService = tradesHandlerService;
            _reportExportingService = reportExportingService;
            _clientLocalTime = clientLocalTime;
        }

        /// <summary>
        /// Handles trade data handlers and csv export handlers.
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        public async Task HandleTradesAndExportReport(CancellationToken stoppingToken)
        {
            try
            {
                var powerTradeDTOs = await _tradesHandlerService.HandleTrades(stoppingToken);
                await _reportExportingService.HandleReportingExportAggregated(powerTradeDTOs, stoppingToken);
                //await _reportExportingService.HandleReportingExportNonAggregated(powerTradeDTOs, stoppingToken);
            }
            catch(Exception ex)
            {
                Log.Error($"[{Assembly.GetEntryAssembly().GetName().Name}] => [{this.GetType().Name}.{ReflectionHelper.GetActualAsyncMethodName()}]" +
                    $" - failed at Client Local Time {_clientLocalTime} with Exception:\n  -Message: {ex.Message}\n  -StackTrace: {ex.StackTrace}");
            }
        }
    }
}
