using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerServiceReporting.UnitTests.AppSettingsValues
{
    public class ExportFilePathUnitTests
    {
        [Fact]
        public void ExportFilePath_Exists_FromAppsettingsDev()
        {
            Environment.SetEnvironmentVariable("APP_ENV", "dev");

            var exportFilePath = GetExportFilePathFromAppsettings();
            bool folderExists = Directory.Exists(exportFilePath);

            Assert.True(folderExists);
        }

        [Fact]
        public void ExportFilePath_Exists_FromAppsettingsRelease()
        {
            Environment.SetEnvironmentVariable("APP_ENV", "release");

            var exportFilePath = GetExportFilePathFromAppsettings();
            bool folderExists = Directory.Exists(exportFilePath);

            Assert.True(folderExists);
        }

        [Fact]
        public void ExportFilePath_Exists_FromAppsettingsProd()
        {
            Environment.SetEnvironmentVariable("APP_ENV", "prod");

            var exportFilePath = GetExportFilePathFromAppsettings();
            bool folderExists = Directory.Exists(exportFilePath);

            Assert.True(folderExists);
        }

        private string GetExportFilePathFromAppsettings()
        {
            var env = Environment.GetEnvironmentVariable("APP_ENV");
            var hostingContext = new HostBuilderContext(new Dictionary<object, object>());
            var configBuilder = new ConfigurationBuilder().AddJsonFile($"appsettings.{env}.json", optional: false, reloadOnChange: false);
            hostingContext.Configuration = configBuilder.Build();
            var section = hostingContext.Configuration.GetSection("TradesReportingWorkerServiceSettings");

            return section.GetValue<string>("ExportFilePath");
        }
    }
}
