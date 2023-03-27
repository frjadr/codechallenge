using PowerServiceReporting.ApplicationCore.DTOs;
using System.Reflection.Metadata;
using System;

namespace PowerServiceReporting.ApplicationCore.Interfaces
{
    /// <summary>
    /// Interface for report export management.
    /// </summary>
    public interface IReportExportingService
    {
        /// <summary>
        /// Handles filtering, mapping and csv export of aggregated data (actuall code challenge requirement).
        /// </summary>
        /// <param name="powerTrades"></param>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        Task HandleReportingExportAggregated(List<PowerTradeDTO> powerTrades, CancellationToken stoppingToken);
        /// <summary>
        /// Handles filtering, mapping and csv export of non agregated data (used for comparison and showcase).
        /// </summary>
        /// <param name="powerTrades"></param>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        Task HandleReportingExportNonAggregated(List<PowerTradeDTO> powerTrades, CancellationToken stoppingToken);
    }
}
