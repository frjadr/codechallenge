namespace PowerServiceReporting.WorkerService.Configurations
{
    /// <summary>
    /// Model for appsetings.json section binding.
    /// </summary>
    public class TradesReportingWorkerServiceSettings
    {
        public string? CronExpression { get; set; }
        public string? TimeZoneId { get; set; }
        public string? ExportFilePath { get; set; }
        public string? ExportFileNamePrefix { get; set; }
    }
}
