using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
