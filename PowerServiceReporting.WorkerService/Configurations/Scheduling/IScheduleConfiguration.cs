using Cronos;

namespace PowerServiceReporting.WorkerService.Configurations.Scheduling
{
    public interface IScheduleConfiguration<T>
    {
        CronExpression CronExpression { get; set; }
        TimeZoneInfo TimeZoneInfo { get; set; }
        DateTime ClientLocalTime { get; set; }
    }
}
