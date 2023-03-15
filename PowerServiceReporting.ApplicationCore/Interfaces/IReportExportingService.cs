using PowerServiceReporting.ApplicationCore.DTOs;

namespace PowerServiceReporting.ApplicationCore.Interfaces
{
    public interface IReportExportingService
    {
        Task HandleReportingExportAggregated(List<PowerTradeDTO> powerTrades, CancellationToken stoppingToken);
        Task HandleReportingExportNonAggregated(List<PowerTradeDTO> powerTrades, CancellationToken stoppingToken);
    }
}
