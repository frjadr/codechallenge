using PowerServiceReporting.ApplicationCore.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerServiceReporting.ApplicationCore.Interfaces
{
    public interface IReportExportingService
    {
        Task HandleReportingExportAggregated(List<PowerTradeDTO> powerTrades, CancellationToken stoppingToken);
        Task HandleReportingExportNonAggregated(List<PowerTradeDTO> powerTrades, CancellationToken stoppingToken);
    }
}
