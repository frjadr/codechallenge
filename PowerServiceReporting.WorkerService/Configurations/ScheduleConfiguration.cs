using Cronos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerServiceReporting.WorkerService.Configurations
{
    public class ScheduleConfiguration<T> : IScheduleConfiguration<T>
    {
        public CronExpression? CronExpression { get; set; }
        public TimeZoneInfo? TimeZone { get; set; }
    }
}
