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
    public class CronExpressionUnitTests
    {
        [Fact]
        public void CronExpression_IsValidExpression_FromAppsettingsDev()
        {
            Environment.SetEnvironmentVariable("APP_ENV", "dev");
        
            var cronExpression = GetCronExpressionFromAppsettings();
            var isValidExpression = ValidateCronExpression(cronExpression);

            Assert.True(isValidExpression);
        }

        [Fact]
        public void CronExpression_IsValidExpression_FromAppsettingsRelease()
        {
            Environment.SetEnvironmentVariable("APP_ENV", "release");
       
            var cronExpression = GetCronExpressionFromAppsettings();
            var isValidExpression = ValidateCronExpression(cronExpression);

            Assert.True(isValidExpression);
        }

        [Fact]
        public void CronExpression_IsValidExpression_FromAppsettingsProd()
        {
            Environment.SetEnvironmentVariable("APP_ENV", "prod");

            var cronExpression = GetCronExpressionFromAppsettings();
            var isValidExpression = ValidateCronExpression(cronExpression);

            Assert.True(isValidExpression);
        }

        private bool ValidateCronExpression(string cronExpression)
        {
            bool isValidExpression = true;
            try
            {
                var expression = CronExpression.Parse(cronExpression, CronFormat.IncludeSeconds);
            }
            catch (CronFormatException ex)
            {
                isValidExpression = false;
            }
            return isValidExpression;
        }

        private string GetCronExpressionFromAppsettings()
        {
            var env = Environment.GetEnvironmentVariable("APP_ENV");
            var hostingContext = new HostBuilderContext(new Dictionary<object, object>());
            var configBuilder = new ConfigurationBuilder().AddJsonFile($"appsettings.{env}.json", optional: false, reloadOnChange: false);
            hostingContext.Configuration = configBuilder.Build();
            var section = hostingContext.Configuration.GetSection("TradesReportingWorkerServiceSettings");
           
            return section.GetValue<string>("CronExpression");
        }
    }
}
