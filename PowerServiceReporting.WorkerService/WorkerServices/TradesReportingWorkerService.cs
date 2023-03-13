  using Cronos;
using PowerServiceReporting.ApplicationCore.Interfaces;

namespace PowerServiceReporting.WorkerService.WorkerServices
{
    public class TradesReportingWorkerService : BaseScheduledBackgroundService
    {
        private ITradesReportingService _tradesReportingService;

        public TradesReportingWorkerService(ITradesReportingService tradesReportingService, CronExpression cronExpression, TimeZoneInfo timeZoneInfo) : base(cronExpression, timeZoneInfo)
        {
            _tradesReportingService = tradesReportingService;
        }

        public override async Task DoWork(CancellationToken stoppingToken)
        {
            try
            {
                await _tradesReportingService.HandleTradesAndExportReport(stoppingToken);
            }
            catch(Exception ex)
            { 

            }
            finally 
            {

            }
        }

        public override Task StartAsync(CancellationToken stoppingToken)
        {
            return base.DoWork(stoppingToken);
        }

        public override Task StopAsync(CancellationToken stoppingToken)
        {
            return base.StopAsync(stoppingToken);
        }
    }
}
