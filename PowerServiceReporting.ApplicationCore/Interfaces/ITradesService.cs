using PowerServiceReporting.ApplicationCore.DTOs;

namespace PowerServiceReporting.ApplicationCore.Interfaces
{
    public interface ITradesService
    {
        Task<List<PowerTradeDTO>> HandleTrades(CancellationToken stoppingToken);
    }
}
