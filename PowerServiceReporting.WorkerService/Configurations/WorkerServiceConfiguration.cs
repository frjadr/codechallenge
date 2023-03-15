using Microsoft.Extensions.DependencyInjection;
using PowerServiceReporting.ApplicationCore.Helpers;
using PowerServiceReporting.WorkerService.Configurations.Scheduling;
using PowerServiceReporting.WorkerService.WorkerServices;
using Serilog;
using System.Reflection;

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

                if (scheduleConfiguration.CronExpression == null || string.IsNullOrEmpty(scheduleConfiguration.CronExpression.ToString()))
                    throw new ArgumentNullException(nameof(options), @"Please provide Cron Expression.");

                if (scheduleConfiguration.TimeZoneInfo == null)
                    throw new ArgumentNullException(nameof(options), @"Please provide Time Zone.");

                services.AddSingleton<IScheduleConfiguration<T>>(scheduleConfiguration);
                services.AddHostedService<T>();
            }
            catch (Exception ex)
            {
                Log.Fatal($"[{Assembly.GetEntryAssembly().GetName().Name}] => [{nameof(Program)}.{ReflectionHelper.GetActualAsyncMethodName()}]" +
                    $" - failed with Exception:\n    -Message: {ex.Message}\n    -StackTrace: {ex.StackTrace}");
            }

            return services;
        }

        public static DateTime LocalClientTime(string timeZoneId)
        {
            Console.WriteLine($"Server time: {DateTime.Now}");

            // Get the current time in server time zone
            DateTime serverUTCTime = DateTime.UtcNow;
            Console.WriteLine($"Server UTC time: {serverUTCTime}");

            // Define the client time zone
            TimeZoneInfo clientTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            Console.WriteLine($"Client Zone: {clientTimeZone}");

            // Convert the server time to client local time
            DateTime clientTime = TimeZoneInfo.ConvertTimeFromUtc(serverUTCTime, clientTimeZone);
            Console.WriteLine($"Client local time: {clientTime}");

            return clientTime;
        }
    }
}
