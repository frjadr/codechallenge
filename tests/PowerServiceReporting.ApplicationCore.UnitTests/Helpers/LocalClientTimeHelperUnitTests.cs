using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using PowerServiceReporting.ApplicationCore.Helpers;

namespace PowerServiceReporting.UnitTests.Helpers
{
    public class LocalClientTimeHelperUnitTests
    {
        [Fact]
        public void LocalClientTime_CorrectTime_ForTimezoneId_FromAppsettingsDev()
        {
            Environment.SetEnvironmentVariable("APP_ENV", "dev");

            TimeComparisonResult timeComparisonResult = GetLocalClientTimeAndExpectedTime();
            var tolerance = TimeSpan.FromSeconds(1);
            var comparisonResult = DateTime.Compare(timeComparisonResult.ExpectedTime, timeComparisonResult.LocalClientTime);

            Assert.InRange(comparisonResult, -1, 1);
            Assert.True(Math.Abs((timeComparisonResult.ExpectedTime - timeComparisonResult.LocalClientTime).TotalSeconds) < tolerance.TotalSeconds);
        }

        [Fact]
        public void LocalClientTime_CorrectTime_ForTimezoneId_FromAppsettingsRelease()
        {
            Environment.SetEnvironmentVariable("APP_ENV", "release");

            TimeComparisonResult timeComparisonResult = GetLocalClientTimeAndExpectedTime();
            var tolerance = TimeSpan.FromSeconds(1);
            var comparisonResult = DateTime.Compare(timeComparisonResult.ExpectedTime, timeComparisonResult.LocalClientTime);

            Assert.InRange(comparisonResult, -1, 1);
            Assert.True(Math.Abs((timeComparisonResult.ExpectedTime - timeComparisonResult.LocalClientTime).TotalSeconds) < tolerance.TotalSeconds);
        }

        [Fact]
        public void LocalClientTime_CorrectTime_ForTimezoneId_FromAppsettingsProd()
        {
            Environment.SetEnvironmentVariable("APP_ENV", "prod");
            
            TimeComparisonResult timeComparisonResult = GetLocalClientTimeAndExpectedTime();
            var tolerance = TimeSpan.FromSeconds(1);
            var comparisonResult = DateTime.Compare(timeComparisonResult.ExpectedTime, timeComparisonResult.LocalClientTime);

            Assert.InRange(comparisonResult, -1, 1);
            Assert.True(Math.Abs((timeComparisonResult.ExpectedTime - timeComparisonResult.LocalClientTime).TotalSeconds) < tolerance.TotalSeconds);
        }

        private TimeComparisonResult GetLocalClientTimeAndExpectedTime()
        {
            var timeZoneId = GetTimeZoneIdFromAppsettings();
            var localClientTime = timeZoneId.LocalClientTime();

            var expectedTimeWithoutSeconds = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time")).AddSeconds(-DateTime.UtcNow.Second);
            var localClientTimeWithoutSeconds = localClientTime.AddSeconds(-localClientTime.Second);
            
            return new TimeComparisonResult
            {
                ExpectedTime = expectedTimeWithoutSeconds,
                LocalClientTime = localClientTimeWithoutSeconds
            };
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

    internal class TimeComparisonResult
    {
        public DateTime ExpectedTime { get; set; }
        public DateTime LocalClientTime { get; set; }
    }
}
