using PowerServiceReporting.ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerServiceReporting.Infrastructure.ServiceImplementations
{
    public class TradesReportingService : ITradesReportingService
    {
        private ITradesService _tradesHandlerService;
        private IReportExportingService _reportExportingService;
      

        public TradesReportingService(ITradesService tradesHandlerService, IReportExportingService reportExportingService)
        {
            _tradesHandlerService = tradesHandlerService;
            _reportExportingService = reportExportingService;
        }

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
                Console.WriteLine($"{ex.Message}\n{ex.StackTrace}");
            }
        }
    }
}
