namespace PowerServiceReporting.ApplicationCore.DTOs
{
    /// <summary>
    /// DTO model for filtered and transformed values from PowerTradeService response DTO.
    /// First two fields are for filtering, last two for csv exoprt.
    /// </summary>
    public class PowerTradeExportDTO
    {
        public DateTime LocalClientTimeOriginal { get; set; }
        public DateTime LocalClientTimeWithPeriod { get; set; }
        public string? Period { get; set; }
        public double Volume { get; set; }
    }
}
