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
            try
            {
                if (options == null)
                    throw new ArgumentNullException(nameof(options), @"Please provide Schedule Configuration.");

                var scheduleConfiguration = new ScheduleConfiguration<T>();
                options.Invoke(scheduleConfiguration);

                if (string.IsNullOrEmpty(scheduleConfiguration.CronExpression.ToString()))
                    throw new ArgumentNullException(nameof(options), @"Please provide Cron Expression.");

                if (scheduleConfiguration.TimeZoneInfo == null)
                    throw new ArgumentNullException(nameof(options), @"Please provide Time Zone.");

                services.AddSingleton<IScheduleConfiguration<T>>(scheduleConfiguration);
                services.AddHostedService<T>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message} + {ex.StackTrace}");
            }

            return services;
        }

        public static DateTime LocalClientTime(string timeZoneId)
        {
            // Get the current time in server time zone
            DateTime serverTime = DateTime.UtcNow;
            Console.WriteLine($"Server time: {serverTime}");

            // Define the client time zone
            TimeZoneInfo clientTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            Console.WriteLine($"Client Zone: {clientTimeZone}");

            // Convert the server time to client local time
            DateTime clientTime = TimeZoneInfo.ConvertTimeFromUtc(serverTime, clientTimeZone);
            Console.WriteLine($"Client local time: {clientTime}");

            return clientTime;
        }
    }
}
