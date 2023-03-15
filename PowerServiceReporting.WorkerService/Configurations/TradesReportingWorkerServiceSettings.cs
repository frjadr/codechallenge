namespace PowerServiceReporting.WorkerService.Configurations
{
    public class TradesReportingWorkerServiceSettings
    {
        public string? CronExpression { get; set; }
        public string? TimeZoneId { get; set; }
        public string? ExportFilePath { get; set; }
        public string? ExportFileNamePrefix { get; set; }
    }
}
