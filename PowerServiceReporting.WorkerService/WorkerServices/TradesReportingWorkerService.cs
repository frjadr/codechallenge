using PowerServiceReporting.ApplicationCore.Helpers;
using PowerServiceReporting.ApplicationCore.Interfaces;
using PowerServiceReporting.WorkerService.Configurations.Scheduling;
using Serilog;
using System.Reflection;

namespace PowerServiceReporting.WorkerService.WorkerServices
{
    public class TradesReportingWorkerService : BaseScheduledBackgroundService
    {
        private ITradesReportingService tradesReportingService;
        private DateTime clientLocalTime;

        public TradesReportingWorkerService(IScheduleConfiguration<TradesReportingWorkerService> scheduleConfiguration, ITradesReportingService tradesReportingService) : base(scheduleConfiguration.CronExpression, scheduleConfiguration.TimeZoneInfo, scheduleConfiguration.ClientLocalTime)
        {
            this.tradesReportingService = tradesReportingService;
            this.clientLocalTime = scheduleConfiguration.ClientLocalTime;
        }

        public override async Task DoWork(CancellationToken stoppingToken)
        {
            try
            {
                Log.Information($"[{Assembly.GetEntryAssembly().GetName().Name}] => [{this.GetType().Name}.{ReflectionHelper.GetActualAsyncMethodName()}] - executing at Client Local Time {clientLocalTime}");
                await tradesReportingService.HandleTradesAndExportReport(stoppingToken);
            }
            catch(Exception ex)
            {
                Log.Fatal($"[{Assembly.GetEntryAssembly().GetName().Name}] => [{this.GetType().Name}.{ReflectionHelper.GetActualAsyncMethodName()}]" +
                    $" - failed at Client Local Time {clientLocalTime} with Exception:\n  -Message: {ex.Message}\n  -StackTrace: {ex.StackTrace}");
            }
            finally 
            {
                Log.Information($"[{Assembly.GetEntryAssembly().GetName().Name}] => [{this.GetType().Name}.{ReflectionHelper.GetActualAsyncMethodName()}] - finished at Client Local Time {clientLocalTime}");
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
