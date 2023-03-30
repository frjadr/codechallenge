using PowerServiceReporting.ApplicationCore.DTOs;
using PowerServiceReporting.ApplicationCore.Helpers;
using PowerServiceReporting.ApplicationCore.Interfaces;
using Serilog;
using System.Collections.Generic;
using System.Reflection;

namespace PowerServiceReporting.Infrastructure.ServiceImplementations
{
    /// <summary>
    /// Class for report export management.
    /// </summary>
    public class ReportExportingService : IReportExportingService
    {
        private readonly string _exportFilePath;
        private readonly string _exportFileNamePrefix;
        private readonly DateTime _clientLocalTime;

        public ReportExportingService(string exportFilePath, string exportFileNamePrefix, DateTime clientLocalTime) 
        { 
            _exportFilePath = exportFilePath;
            _exportFileNamePrefix = exportFileNamePrefix;  
            _clientLocalTime = clientLocalTime;
        }

        /// <summary>
        /// Handles filtering, mapping and csv export of aggregated data (actuall code challenge requirement).
        /// </summary>
        /// <param name="powerTrades"></param>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        public async Task HandleReportingExportAggregated(List<PowerTradeDTO> powerTrades, CancellationToken stoppingToken)
        {
            try
            {
                var fullExportFilePath = _exportFilePath.HandleFolderAndFilePathAggregated(_exportFileNamePrefix, _clientLocalTime);
                var powerTradesExport = powerTrades.MapPowerTradesToPowerTradesExportAggregated(_clientLocalTime);
                powerTradesExport.ExportPowerTradesToCSV(fullExportFilePath);
            }
            catch (Exception ex)
            {
                Log.Error($"[{Assembly.GetEntryAssembly().GetName().Name}] => [{this.GetType().Name}.{ReflectionHelper.GetActualAsyncMethodName()}]" +
                    $" - failed at Client Local Time {_clientLocalTime} with Exception:\n  -Message: {ex.Message}\n  -StackTrace: {ex.StackTrace}");
            }
            await Task.CompletedTask;
        }

        /// <summary>
        /// Handles filtering, mapping and csv export of non agregated data (used for comparison and showcase).
        /// </summary>
        /// <param name="powerTrades"></param>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        public async Task HandleReportingExportNonAggregated(List<PowerTradeDTO> powerTrades, CancellationToken stoppingToken)
        {
            try
            {
                var fullExportFilePath = _exportFilePath.HandleFolderAndFilePathNonAggregated(_exportFileNamePrefix, _clientLocalTime);
                var powerTradesExport = powerTrades.MapPowerTradesToPowerTradesExportNonAggregated(_clientLocalTime);
                powerTradesExport.ExportPowerTradesToCSV(fullExportFilePath);
            }
            catch (Exception ex)
            {
                Log.Error($"[{Assembly.GetEntryAssembly().GetName().Name}] => [{this.GetType().Name}.{ReflectionHelper.GetActualAsyncMethodName()}]" +
                    $" - failed at Client Local Time {_clientLocalTime} with Exception:\n  -Message: {ex.Message}\n  -StackTrace: {ex.StackTrace}");
            }
            await Task.CompletedTask;
        }
    }
}
