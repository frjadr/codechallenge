using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerServiceReporting.ApplicationCore.Interfaces
{
    /// <summary>
    /// Interface for top lvl business logic class used by Worker Service.
    /// </summary>
    public interface ITradesReportingService
    {
        /// <summary>
        /// Handles trade data handlers and csv export handlers.
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        Task HandleTradesAndExportReport(CancellationToken stoppingToken);
    }
}
