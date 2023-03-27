using Cronos;

namespace PowerServiceReporting.WorkerService.Configurations.Scheduling
{
    /// <summary>
    /// Schedule configuration class with objects.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ScheduleConfiguration<T> : IScheduleConfiguration<T>
    {
        public CronExpression? CronExpression { get; set; }
        public TimeZoneInfo? TimeZoneInfo { get; set; }
        public DateTime ClientLocalTime { get; set; }
    }
}
