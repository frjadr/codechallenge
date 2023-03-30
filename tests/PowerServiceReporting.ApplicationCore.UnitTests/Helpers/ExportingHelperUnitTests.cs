using PowerServiceReporting.ApplicationCore.DTOs;
using PowerServiceReporting.ApplicationCore.Helpers;

namespace PowerServiceReporting.UnitTests.Helpers
{
    public class ExportingHelperUnitTests
    {
        [Fact]
        public void HandleFolderAndFilePath_FolderExists_And_ReturnsCorrectFilePath_Aggregated()
        {
            string expectedExportFilePath = @"C:\PowerReportFolder\PowerPosition_20230329_0830.csv";
            string exportFilePath = @"C:\PowerReportFolder\";
            string exportFileNamePrefix = "PowerPosition";
            DateTime clientLocalTime = new DateTime(2023, 03, 29, 08, 30, 0);

            string actualFilePath = exportFilePath.HandleFolderAndFilePathAggregated(exportFileNamePrefix, clientLocalTime);

            Assert.True(Directory.Exists(exportFilePath));
            Assert.Equal(expectedExportFilePath, actualFilePath);
        }

        [Fact]
        public void HandleFolderAndFilePath_FolderExists_And_ReturnsCorrectFilePath_NonAggregated()
        {
            string expectedExportFilePath = @"C:\PowerReportFolder\PowerPosition_20230329_0830_NONAGGREGATED.csv";
            string exportFilePath = @"C:\PowerReportFolder\";
            string exportFileNamePrefix = "PowerPosition";
            DateTime clientLocalTime = new DateTime(2023, 03, 29, 08, 30, 0);

            string actualFilePath = exportFilePath.HandleFolderAndFilePathNonAggregated(exportFileNamePrefix, clientLocalTime);

            Assert.True(Directory.Exists(exportFilePath));
            Assert.Equal(expectedExportFilePath, actualFilePath);
        }

        [Fact]
        public void ExportPowerTradesToCSV_ShouldExport_PowerTradesToCSVFile()
        {
            List<PowerTradeExportDTO> powerTradeExportDTOs = new List<PowerTradeExportDTO>()
            {
                new PowerTradeExportDTO { Period = "23:00", Volume = 323.21 },
                new PowerTradeExportDTO { Period = "00:00", Volume = 112.32 },
                new PowerTradeExportDTO { Period = "01:00", Volume = 231.13 }
            };
            string filePath =  @"C:\PowerReportFolder\PowerPosition_20230329_0830.csv";

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
