﻿using AutoMapper;
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
            var randomDay = _clientLocalTime;
            var sourceList = new List<PowerTradeMock>()
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
            var destinationList = _mapper.MapList<PowerTradeMock, PowerTradeDTO>(sourceList);

            Assert.Equal(sourceList.Count, destinationList.Count);

            Assert.Equal(sourceList[0].Periods[0].Period, destinationList[0].Periods[0].Period);
            Assert.Equal(sourceList[0].Periods[0].Volume, destinationList[0].Periods[0].Volume);

            Assert.Equal(sourceList[0].Periods[1].Period, destinationList[0].Periods[1].Period);
            Assert.Equal(sourceList[0].Periods[1].Volume, destinationList[0].Periods[1].Volume);

            Assert.Equal(sourceList[0].Periods[2].Period, destinationList[0].Periods[2].Period);
            Assert.Equal(sourceList[0].Periods[2].Volume, destinationList[0].Periods[2].Volume);


            Assert.Equal(sourceList[1].Periods[0].Period, destinationList[1].Periods[0].Period);
            Assert.Equal(sourceList[1].Periods[0].Volume, destinationList[1].Periods[0].Volume);

            Assert.Equal(sourceList[1].Periods[1].Period, destinationList[1].Periods[1].Period);
            Assert.Equal(sourceList[1].Periods[1].Volume, destinationList[1].Periods[1].Volume);

            Assert.Equal(sourceList[1].Periods[2].Period, destinationList[1].Periods[2].Period);
            Assert.Equal(sourceList[1].Periods[2].Volume, destinationList[1].Periods[2].Volume);
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
