using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerServiceReporting.ApplicationCore.Interfaces
{
    public interface IReportExportingService
    {
        Task HandleReportingExport(CancellationToken stoppingToken);
    }
}
