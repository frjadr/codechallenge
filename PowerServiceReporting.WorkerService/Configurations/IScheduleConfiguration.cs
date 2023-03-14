using Cronos;

namespace PowerServiceReporting.WorkerService.Configurations
{
    public interface IScheduleConfiguration<T>
    {
        CronExpression CronExpression { get; set; }
        TimeZoneInfo TimeZoneInfo { get; set; } 
    }
}
