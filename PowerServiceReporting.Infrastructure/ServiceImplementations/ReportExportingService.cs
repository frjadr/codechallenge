using PowerServiceReporting.ApplicationCore.DTOs;
using PowerServiceReporting.ApplicationCore.Helpers;
using PowerServiceReporting.ApplicationCore.Interfaces;
using Serilog;
using System.Reflection;

namespace PowerServiceReporting.Infrastructure.ServiceImplementations
{
    /// <summary>
    /// Class for report export management.
    /// </summary>
    public class ReportExportingService : IReportExportingService
    {
        private string exportFilePath { get; set; }
        private string exportFileNamePrefix { get; set; }
        private DateTime clientLocalTime { get; set; }

        public ReportExportingService(string exportFilePath, string exportFileNamePrefix, DateTime clientLocalTime) 
        { 
            this.exportFilePath = exportFilePath;
            this.exportFileNamePrefix = exportFileNamePrefix;  
            this.clientLocalTime = clientLocalTime;
        }

        /// <summary>
        /// Handles filtering, mapping and csv export of aggregated data (actuall code challenge requirement).
        /// </summary>
        /// <param name="powerTrades"></param>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        public async Task HandleReportingExportAggregated(List<PowerTradeDTO> powerTrades, CancellationToken stoppingToken)
        {
            var filePath = Path.Combine(exportFilePath, $"{exportFileNamePrefix}_{clientLocalTime:yyyyMMdd_HHmm}.csv");
            try
            {
                var powerTradesExport = powerTrades.MapPowerTradesToPowerTradesExportAggregated(clientLocalTime);
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.WriteLine("Local Time,Volume");
                    powerTradesExport.ForEach(data => writer.WriteLine($"{data.Period},{data.Volume}"));
                }
            }
            catch (Exception ex)
            {
                Log.Error($"[{Assembly.GetEntryAssembly().GetName().Name}] => [{this.GetType().Name}.{ReflectionHelper.GetActualAsyncMethodName()}]" +
                    $" - failed at Client Local Time {clientLocalTime} with Exception:\n  -Message: {ex.Message}\n  -StackTrace: {ex.StackTrace}");
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
            var filePath = Path.Combine(exportFilePath, $"{exportFileNamePrefix}_{clientLocalTime:yyyyMMdd_HHmm}_NONAGGREGATED.csv");
            try
            {
                var powerTradesExport = powerTrades.MapPowerTradesToPowerTradesExportNonAggregated(clientLocalTime);
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.WriteLine("Local Time,Volume");
                    powerTradesExport.ForEach(data => writer.WriteLine($"{data.Period},{data.Volume}"));
                }
            }
            catch (Exception ex)
            {
                Log.Error($"[{Assembly.GetEntryAssembly().GetName().Name}] => [{this.GetType().Name}.{ReflectionHelper.GetActualAsyncMethodName()}]" +
                    $" - failed at Client Local Time {clientLocalTime} with Exception:\n  -Message: {ex.Message}\n  -StackTrace: {ex.StackTrace}");
            }
            await Task.CompletedTask;
        }
    }
}
