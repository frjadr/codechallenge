  using Cronos;
using PowerServiceReporting.ApplicationCore.Interfaces;
using PowerServiceReporting.WorkerService.Configurations;

namespace PowerServiceReporting.WorkerService.WorkerServices
{
    public class TradesReportingWorkerService : BaseScheduledBackgroundService
    {
        private ITradesReportingService _tradesReportingService;

        public TradesReportingWorkerService(IScheduleConfiguration<TradesReportingWorkerService> scheduleConfiguration, ITradesReportingService tradesReportingService) : base(scheduleConfiguration.CronExpression, scheduleConfiguration.TimeZoneInfo)
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
                Console.WriteLine($"{ex.Message}\n{ex.StackTrace}");
            }
            finally 
            {

            }
        }

        public override Task StartAsync(CancellationToken stoppingToken)
        {
            return base.StartAsync(stoppingToken);
        }

        public override Task StopAsync(CancellationToken stoppingToken)
        {
            return base.StopAsync(stoppingToken);
        }
    }
}
