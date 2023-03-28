using AutoMapper;
using PowerServiceReporting.ApplicationCore.DTOs;
using PowerServiceReporting.ApplicationCore.Helpers;

namespace PowerServiceReporting.UnitTests.Helpers
{
    public class AutoMapperHelperUnitTest 
    { 
        private readonly IConfigurationProvider _configuration;
        private readonly IMapper _mapper;
        private readonly DateTime _clientLocalTime;
        private const string TimeZoneId = "GMT Standard Time";

        public AutoMapperHelperUnitTest()
        {
            _clientLocalTime = TimeZoneId.LocalClientTime();
            _configuration = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfileMock(_clientLocalTime)));
            _mapper = _configuration.CreateMapper();     
        }

        [Fact]
        public void MapList_ShouldReturn_CorrectlyMappedList()
        {
            var powerTradeMocks = new List<PowerTradeMock>()
            {
                new PowerTradeMock()
                {
                    Date = _clientLocalTime,
                    Periods = new PowerPeriodMock[] 
                    {  
                        new PowerPeriodMock() { Period = 0, Volume = 23.5512 },
                        new PowerPeriodMock() { Period = 1, Volume = 32.1255 },
                        new PowerPeriodMock() { Period = 1, Volume = 12.5532 }
                    }
                },
                new PowerTradeMock()
                {
                    Date = _clientLocalTime,
                    Periods = new PowerPeriodMock[]
                    {
                        new PowerPeriodMock() { Period = 0, Volume = 12.5532 },
                        new PowerPeriodMock() { Period = 1, Volume = 32.1255 },
                        new PowerPeriodMock() { Period = 1, Volume = 23.5512 }
                    }
                },
            };
            var mappedPowerTrades = _mapper.MapList<PowerTradeMock, PowerTradeDTO>(powerTradeMocks);

            Assert.Equal(powerTradeMocks.Count, mappedPowerTrades.Count);

            Assert.Equal(powerTradeMocks[0].Periods[0].Period, mappedPowerTrades[0].Periods[0].Period);
            Assert.Equal(powerTradeMocks[0].Periods[0].Volume, mappedPowerTrades[0].Periods[0].Volume);

            Assert.Equal(powerTradeMocks[0].Periods[1].Period, mappedPowerTrades[0].Periods[1].Period);
            Assert.Equal(powerTradeMocks[0].Periods[1].Volume, mappedPowerTrades[0].Periods[1].Volume);

            Assert.Equal(powerTradeMocks[0].Periods[2].Period, mappedPowerTrades[0].Periods[2].Period);
            Assert.Equal(powerTradeMocks[0].Periods[2].Volume, mappedPowerTrades[0].Periods[2].Volume);

            Assert.Equal(powerTradeMocks[1].Periods[0].Period, mappedPowerTrades[1].Periods[0].Period);
            Assert.Equal(powerTradeMocks[1].Periods[0].Volume, mappedPowerTrades[1].Periods[0].Volume);

            Assert.Equal(powerTradeMocks[1].Periods[1].Period, mappedPowerTrades[1].Periods[1].Period);
            Assert.Equal(powerTradeMocks[1].Periods[1].Volume, mappedPowerTrades[1].Periods[1].Volume);

            Assert.Equal(powerTradeMocks[1].Periods[2].Period, mappedPowerTrades[1].Periods[2].Period);
            Assert.Equal(powerTradeMocks[1].Periods[2].Volume, mappedPowerTrades[1].Periods[2].Volume);
        }
    }

    internal class MappingProfileMock : Profile
    {
        public MappingProfileMock(DateTime clientLocalTime)
        {
            CreateMap<PowerPeriodMock, PowerPeriodDTO>();
            CreateMap<PowerTradeMock, PowerTradeDTO>();
        }
    }

    internal class PowerTradeMock
    {
        public DateTime Date { get; set; }
        public PowerPeriodMock[] Periods { get; set; } = new PowerPeriodMock[0];
    }

    internal class PowerPeriodMock
    {
        public int Period { get; set; }
        public double Volume { get; set; }
    }
}
