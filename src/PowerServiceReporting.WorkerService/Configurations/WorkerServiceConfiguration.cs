using Microsoft.Extensions.DependencyInjection;
using PowerServiceReporting.ApplicationCore.Helpers;
using PowerServiceReporting.WorkerService.Configurations.Scheduling;
using PowerServiceReporting.WorkerService.WorkerServices;
using Serilog;
using System.Reflection;

namespace PowerServiceReporting.WorkerService.Configurations
{
    /// <summary>
    /// Scheduled worker service configuration class.
    /// </summary>
    public static class WorkerServiceConfiguration
    {
        /// <summary>
        /// Adds schedule configuration and hosted service to service collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="services"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
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
    }
}
