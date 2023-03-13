using PowerServiceReporting.ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerServiceReporting.Infrastructure.ServiceImplementations
{
    public class ReportExportingService : IReportExportingService
    {
        public Task HandleReportingExport(CancellationToken stoppingToken)
        {
            throw new NotImplementedException();
        }
    }
}
