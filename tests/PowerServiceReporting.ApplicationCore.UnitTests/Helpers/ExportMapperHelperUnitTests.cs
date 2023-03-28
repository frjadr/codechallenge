using PowerServiceReporting.ApplicationCore.DTOs;
using PowerServiceReporting.ApplicationCore.Helpers;

namespace PowerServiceReporting.UnitTests.Helpers
{
    public class ExportMapperHelperUnitTests
    {
        private const string TimeZoneId = "GMT Standard Time";
        private readonly DateTime _clientLocalTime;
        private List<PowerTradeDTO> powerTrades;

        public ExportMapperHelperUnitTests()
        {
            _clientLocalTime = TimeZoneId.LocalClientTime();
            powerTrades = InitializePowerTradeDTOs(_clientLocalTime);
        }

        [Fact]
        public void MapPowerTradesToPowerTradesExportAggregated_ShouldReturn_AggregatedData_WithCorrectSumAndPositioning()
        {
            var result = ExportMapperHelper.MapPowerTradesToPowerTradesExportAggregated(powerTrades, _clientLocalTime);

            Assert.Equal("23:00", result[0].Period); // should end with 23:00 period
            Assert.Equal("00:00", result[1].Period); // should start with 00:00 period
            Assert.Equal("22:00", result[2].Period); // should start with 22:00 period

            // should aggregate volumes for each period
            Assert.Equal(23.5512 + 12.5532, result[0].Volume);  
            Assert.Equal(32.1255 + 32.1255, result[1].Volume);
            Assert.Equal(12.5532 + 23.5512, result[2].Volume);
        }

        [Fact]
        public void MapPowerTradesToPowerTradesExportNonAggregated_ShouldReturn_NonAggregatedData_WithCorrectPositioning()
        {
            var result = ExportMapperHelper.MapPowerTradesToPowerTradesExportNonAggregated(powerTrades, _clientLocalTime);

            Assert.Equal("23:00", result[0].Period); // should end with 23:00 period
            Assert.Equal("23:00", result[1].Period); // should end with 23:00 period
            Assert.Equal("00:00", result[2].Period); // should start with 00:00 period
            Assert.Equal("00:00", result[3].Period); // should start with 00:00 period
            Assert.Equal("22:00", result[4].Period); // should start with 22:00 period
            Assert.Equal("22:00", result[5].Period); // should start with 22:00 period
        }

        private List<PowerTradeDTO> InitializePowerTradeDTOs(DateTime clientLocalTime)
        {
            var powerTrades = new List<PowerTradeDTO>
            {
                new PowerTradeDTO
                {
                    Date = clientLocalTime,
                    Periods = new PowerPeriodDTO[]
                    {
                        new PowerPeriodDTO() { Period = 1, Volume = 23.5512 },
                        new PowerPeriodDTO() { Period = 0, Volume = 32.1255 },
                        new PowerPeriodDTO() { Period = 24, Volume = 12.5532 }
                    }
                },
                new PowerTradeDTO
                {
                    Date = clientLocalTime,
                    Periods = new PowerPeriodDTO[]
                    {
                        new PowerPeriodDTO() { Period = 1, Volume = 12.5532 },
                        new PowerPeriodDTO() { Period = 0, Volume = 32.1255 },
                        new PowerPeriodDTO() { Period = 24, Volume = 23.5512 }
                    }
                }
            };

            return powerTrades;
        }
    }
}
