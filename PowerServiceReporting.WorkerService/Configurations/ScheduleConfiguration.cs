using Cronos;

namespace PowerServiceReporting.WorkerService.Configurations
{
    public class ScheduleConfiguration<T> : IScheduleConfiguration<T>
    {
        public CronExpression? CronExpression { get; set; }
        public TimeZoneInfo? TimeZoneInfo { get; set; }
    }
}
