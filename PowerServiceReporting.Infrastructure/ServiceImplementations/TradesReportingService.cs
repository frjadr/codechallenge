using PowerServiceReporting.ApplicationCore.Helpers;
using PowerServiceReporting.ApplicationCore.Interfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PowerServiceReporting.Infrastructure.ServiceImplementations
{
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

        public async Task HandleTradesAndExportReport(CancellationToken stoppingToken)
        {
            try
            {
                var powerTradeDTOs = await tradesHandlerService.HandleTrades(stoppingToken);
                await reportExportingService.HandleReportingExportAggregated(powerTradeDTOs, stoppingToken);
                //await _reportExportingService.HandleReportingExportNonAggregated(powerTradeDTOs, stoppingToken);
            }
            catch(Exception ex)
            {
                Log.Error($"[{Assembly.GetEntryAssembly().GetName().Name}] => [{this.GetType().Name}.{ReflectionHelper.GetActualAsyncMethodName()}]" +
                    $" - failed at Client Local Time {clientLocalTime} with Exception:\n  -Message: {ex.Message}\n  -StackTrace: {ex.StackTrace}");
            }
        }
    }
}
