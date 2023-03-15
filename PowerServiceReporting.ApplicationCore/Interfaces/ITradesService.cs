using PowerServiceReporting.ApplicationCore.DTOs;

namespace PowerServiceReporting.ApplicationCore.Interfaces
{
    /// <summary>
    /// Interface for trades fetching and mapping.
    /// </summary>
    public interface ITradesService
    {
        /// <summary>
        /// Handles randomly generated trade data from PowerService and maps them to DTO.
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        Task<List<PowerTradeDTO>> HandleTrades(CancellationToken stoppingToken);
    }
}
