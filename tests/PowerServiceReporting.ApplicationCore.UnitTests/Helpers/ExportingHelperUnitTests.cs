using PowerServiceReporting.ApplicationCore.DTOs;
using PowerServiceReporting.ApplicationCore.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerServiceReporting.UnitTests.Helpers
{
    public class ExportingHelperUnitTests
    {

        [Fact]
        public void ExportPowerTradesToCSV_ShouldExport_PowerTradesToCSVFile()
        {
            List<PowerTradeExportDTO> powerTradeExportDTOs = new List<PowerTradeExportDTO>()
            {
                new PowerTradeExportDTO { Period = "23:00", Volume = 323.21 },
                new PowerTradeExportDTO { Period = "00:00", Volume = 112.32 },
                new PowerTradeExportDTO { Period = "01:00", Volume = 231.13 }
            };
            string filePath = "C:\\PowerReportFolder\\PowerPosition_20230329_0830.csv";

            powerTradeExportDTOs.ExportPowerTradesToCSV(filePath);

            Assert.True(File.Exists(filePath));         
            string[] lines = File.ReadAllLines(filePath);
            Assert.Equal("Local Time,Volume", lines[0]); // Check the header line
            Assert.Equal("23:00,323.21", lines[1]); // Check the first data line
            Assert.Equal("00:00,112.32", lines[2]); // Check the second data line
            Assert.Equal("01:00,231.13", lines[3]); // Check the last data line
        }
    }
}
