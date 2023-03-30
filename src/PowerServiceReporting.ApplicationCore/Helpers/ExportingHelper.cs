using PowerServiceReporting.ApplicationCore.DTOs;

namespace PowerServiceReporting.ApplicationCore.Helpers
{
    public static class ExportingHelper
    {
        public static string HandleFolderAndFilePathNonAggregated(this string exportFilePath, string exportFileNamePrefix, DateTime clientLocalTime)
        {
            if (!Directory.Exists(exportFilePath))
                Directory.CreateDirectory(exportFilePath);
            return Path.Combine(exportFilePath, $"{exportFileNamePrefix}_{clientLocalTime:yyyyMMdd_HHmm}_NONAGGREGATED.csv");
        }

        public static string HandleFolderAndFilePathAggregated(this string exportFilePath, string exportFileNamePrefix, DateTime clientLocalTime)
        {
            if (!Directory.Exists(exportFilePath))
                Directory.CreateDirectory(exportFilePath);
            return Path.Combine(exportFilePath, $"{exportFileNamePrefix}_{clientLocalTime:yyyyMMdd_HHmm}.csv");
        }

        public static void ExportPowerTradesToCSV(this List<PowerTradeExportDTO> powerTradesExport, string fullExportFilePath)
        {
            using (StreamWriter writer = new StreamWriter(fullExportFilePath))
            {
                writer.WriteLine("Local Time,Volume");
                powerTradesExport.ForEach(data => writer.WriteLine($"{data.Period},{data.Volume}"));
            }
        }
    }
}
