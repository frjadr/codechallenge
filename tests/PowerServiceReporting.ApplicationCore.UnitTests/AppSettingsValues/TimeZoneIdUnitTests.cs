using Cronos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerServiceReporting.UnitTests.AppSettingsValues
{
    public class TimeZoneIdUnitTests
    {
        [Fact]
        public void TimeZoneId_IsValidTimeZoneId_FromAppsettingsDev()
        {
            Environment.SetEnvironmentVariable("APP_ENV", "dev");

            var timeZoneId = GetTimeZoneIdFromAppsettings();
            var isValidTimeZoneId = ValidateTimeZoneId(timeZoneId);

            Assert.True(isValidTimeZoneId);
        }

        [Fact]
        public void TimeZoneId_IsValidTimeZoneId_FromAppsettingsTest()
        {
            Environment.SetEnvironmentVariable("APP_ENV", "release");

            var timeZoneId = GetTimeZoneIdFromAppsettings();
            var isValidTimeZoneId = ValidateTimeZoneId(timeZoneId);
            Assert.True(isValidTimeZoneId);
        }

        [Fact]
        public void TimeZoneId_IsValidTimeZoneId_FromAppsettingsProd()
        {
            Environment.SetEnvironmentVariable("APP_ENV", "prod");

            var timeZoneId = GetTimeZoneIdFromAppsettings();
            var isValidTimeZoneId = ValidateTimeZoneId(timeZoneId);

            Assert.True(isValidTimeZoneId);
        }

        private bool ValidateTimeZoneId(string timeZoneId)
        {
            bool isValidTimeZoneId = true;
            TimeZoneInfo timeZone;
            try
            {
                timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            }
            catch (Exception ex)
            {
                isValidTimeZoneId = false;
            }
            return isValidTimeZoneId;
        }


        private string GetTimeZoneIdFromAppsettings()
        {
            var env = Environment.GetEnvironmentVariable("APP_ENV");
            var hostingContext = new HostBuilderContext(new Dictionary<object, object>());
            var configBuilder = new ConfigurationBuilder().AddJsonFile($"appsettings.{env}.json", optional: false, reloadOnChange: false);
            hostingContext.Configuration = configBuilder.Build();
            var section = hostingContext.Configuration.GetSection("TradesReportingWorkerServiceSettings");

            return section.GetValue<string>("TimeZoneId");
        }
    }
}
