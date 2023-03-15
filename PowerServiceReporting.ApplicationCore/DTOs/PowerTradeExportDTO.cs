namespace PowerServiceReporting.ApplicationCore.DTOs
{
    public class PowerTradeExportDTO
    {
        public DateTime LocalClientTimeOriginal { get; set; }
        public DateTime LocalClientTimeWithPeriod { get; set; }
        public string? Period { get; set; }
        public double Volume { get; set; }
    }
}
