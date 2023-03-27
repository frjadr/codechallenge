using Cronos;

namespace PowerServiceReporting.WorkerService.Configurations.Scheduling
{
    /// <summary>
    /// Schedule configuration interface with objects.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IScheduleConfiguration<T>
    {
        CronExpression CronExpression { get; set; }
        TimeZoneInfo TimeZoneInfo { get; set; }
        DateTime ClientLocalTime { get; set; }
    }
}
