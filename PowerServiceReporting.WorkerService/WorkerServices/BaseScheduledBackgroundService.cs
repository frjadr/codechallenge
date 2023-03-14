using Cronos;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerServiceReporting.WorkerService.WorkerServices
{
    public abstract class BaseScheduledBackgroundService : BackgroundService
    {
        private readonly CronExpression cronExpression;
        private readonly TimeZoneInfo timeZoneInfo;
        private System.Timers.Timer? timer;
        protected DateTimeOffset? nextOccurence;

        protected BaseScheduledBackgroundService(CronExpression cronExpression, TimeZoneInfo timeZone)
        {
            this.cronExpression = cronExpression;
            this.timeZoneInfo = timeZone;
        }

        protected virtual async Task ScheduleJob(CancellationToken stoppingToken)
        {
            nextOccurence = cronExpression.GetNextOccurrence(DateTimeOffset.Now, timeZoneInfo);
            if (nextOccurence.HasValue)
            {
                var delay = nextOccurence.Value - DateTimeOffset.Now;
                if (delay.TotalMilliseconds <= 0)
                    await ScheduleJob(stoppingToken);

                timer = new System.Timers.Timer(delay.TotalMilliseconds);
                timer.Elapsed += async (sender, elapsedEventArgs) =>
                {
                    timer.Dispose();
                    timer = null;

                    if (!stoppingToken.IsCancellationRequested)
                        await DoWork(stoppingToken);
                    if (!stoppingToken.IsCancellationRequested)
                        await ScheduleJob(stoppingToken);
                };
                timer.Start();        
            }

            await Task.CompletedTask;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken) => await ScheduleJob(stoppingToken);
        
        public virtual async Task DoWork(CancellationToken stoppingToken) => await Task.Delay(5000, stoppingToken);
        
        public override async Task StartAsync(CancellationToken stoppingToken) => await ScheduleJob(stoppingToken);
        
        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            timer?.Stop();
            await Task.CompletedTask;
        }

        public override void Dispose() => timer?.Dispose();
    }
}
