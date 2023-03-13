using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerServiceReporting.ApplicationCore.Interfaces
{
    public interface ITradesReportingService
    {
        Task HandleTradesAndExportReport(CancellationToken stoppingToken);
    }
}
