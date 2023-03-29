using PowerServiceReporting.ApplicationCore.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerServiceReporting.ApplicationCore.Helpers
{
    public static class ExportingHelper
    {
        public static void ExportPowerTradesToCSV(this List<PowerTradeExportDTO> powerTradesExport, string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("Local Time,Volume");
                powerTradesExport.ForEach(data => writer.WriteLine($"{data.Period},{data.Volume}"));
            }
        }
    }
}
