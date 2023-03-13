using Cronos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerServiceReporting.WorkerService.Configurations
{
    public interface IScheduleConfiguration<T>
    {
        CronExpression? CronExpression { get; set; }
        TimeZoneInfo? TimeZone { get; set; } 
    }
}
