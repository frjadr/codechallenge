using PowerServiceReporting.ApplicationCore.DTOs;
using PowerServiceReporting.ApplicationCore.Helpers;
using PowerServiceReporting.ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PowerServiceReporting.Infrastructure.ServiceImplementations
{
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

        public async Task HandleReportingExportAggregated(List<PowerTradeDTO> powerTrades, CancellationToken stoppingToken)
        {
            var filePath = Path.Combine(exportFilePath, $"{exportFileNamePrefix}_{clientLocalTime:yyyyMMdd_HHmm}.csv");
            try
            {
                var powerTradesExport = powerTrades.MapPowerTradesToPowerTradesExportAggregated();
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.WriteLine("Local Time,Volume");
                    powerTradesExport.ForEach(data => writer.WriteLine($"{data.Period},{data.Volume}"));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}\n{ex.StackTrace}");
            }
            await Task.CompletedTask;
        }

        public async Task HandleReportingExportNonAggregated(List<PowerTradeDTO> powerTrades, CancellationToken stoppingToken)
        {
            var filePath = Path.Combine(exportFilePath, $"{exportFileNamePrefix}_{clientLocalTime:yyyyMMdd_HHmm}_NONAGGREGATED.csv");
            try
            {
                var powerTradesExport = powerTrades.MapPowerTradesToPowerTradesExportNonAggregated();
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.WriteLine("Local Time,Volume");
                    powerTradesExport.ForEach(data => writer.WriteLine($"{data.Period},{data.Volume}"));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}\n{ex.StackTrace}");
            }
            await Task.CompletedTask;
        }
    }
}
