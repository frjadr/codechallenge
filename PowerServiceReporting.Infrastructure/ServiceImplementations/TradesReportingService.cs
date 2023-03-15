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
        private ITradesService tradesHandlerService;
        private IReportExportingService reportExportingService;
        private DateTime clientLocalTime;

        public TradesReportingService(ITradesService tradesHandlerService, IReportExportingService reportExportingService, DateTime clientLocalTime)
        {
            this.tradesHandlerService = tradesHandlerService;
            this.reportExportingService = reportExportingService;
            this.clientLocalTime = clientLocalTime;
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
                var powerTradeDTOs = await tradesHandlerService.HandleTrades(stoppingToken);
                await reportExportingService.HandleReportingExportAggregated(powerTradeDTOs, stoppingToken);
                //await reportExportingService.HandleReportingExportNonAggregated(powerTradeDTOs, stoppingToken);
            }
            catch(Exception ex)
            {
                Log.Error($"[{Assembly.GetEntryAssembly().GetName().Name}] => [{this.GetType().Name}.{ReflectionHelper.GetActualAsyncMethodName()}]" +
                    $" - failed at Client Local Time {clientLocalTime} with Exception:\n  -Message: {ex.Message}\n  -StackTrace: {ex.StackTrace}");
            }
        }
    }
}
