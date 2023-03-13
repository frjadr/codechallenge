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
                await _tradesHandlerService.HandleTrades(stoppingToken);
                await _reportExportingService.HandleReportingExport(stoppingToken);
            }
            catch(Exception ex)
            {

            }
        }
    }
}
