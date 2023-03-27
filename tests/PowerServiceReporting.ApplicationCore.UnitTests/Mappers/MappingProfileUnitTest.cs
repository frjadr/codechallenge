using AutoMapper;
using PowerServiceReporting.ApplicationCore.DTOs;
using PowerServiceReporting.ApplicationCore.Mappers;
using Services;
using System.Runtime.Serialization;

namespace PowerServiceReporting.ApplicationCore.UnitTests.Mappers
{
    public class MappingProfileUnitTest
    {
        private readonly IConfigurationProvider _configuration;
        private readonly IMapper _mapper;

        public MappingProfileUnitTest()
        {
            _configuration = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile(default)));
            _mapper = _configuration.CreateMapper();
        }

        [Fact]
        public void Configuration_IsValid() =>  _configuration.AssertConfigurationIsValid();

        [Theory]
        [InlineData(typeof(PowerPeriod), typeof(PowerPeriodDTO))]
        [InlineData(typeof(PowerTrade), typeof(PowerTradeDTO))]
        public void Map_ShouldSupport_FromSourceToDestination(Type source, Type destination) => _mapper.Map(source, destination);
    }
}