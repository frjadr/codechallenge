using Microsoft.Extensions.DependencyInjection;
using PowerServiceReporting.WorkerService.WorkerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerServiceReporting.WorkerService.Configurations
{
    public static class WorkerServiceConfiguration
    {

        public static IServiceCollection AddCronScheduledHostedWorkerService<T>(this IServiceCollection services, Action<IScheduleConfiguration<T>> options) where T : BaseScheduledBackgroundService
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options), @"Please provide Schedule Configuration.");

            var scheduleConfiguration = new ScheduleConfiguration<T>();
            options.Invoke(scheduleConfiguration);

            if (string.IsNullOrEmpty(scheduleConfiguration.CronExpression.ToString()))
                throw new ArgumentNullException(nameof(options), @"Please provide Cron Expression.");

            if (scheduleConfiguration.TimeZone == null)
                throw new ArgumentNullException(nameof(options), @"Please provide Time Zone.");

            services.AddSingleton<IScheduleConfiguration<T>>(scheduleConfiguration);
            services.AddHostedService<T>();

            return services;
        }
    }
}
