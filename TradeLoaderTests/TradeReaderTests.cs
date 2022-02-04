using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeLoader;
using TradeLoader.Profiles;
using Xunit;

namespace TradeLoaderTests
{
    public class TradeReaderTests
    {
        private readonly IServiceProvider _serviceProvider;

        public TradeReaderTests()
        {
            var _simpleValidator = new SimpleValidator(Mock.Of<ILogger<SimpleValidator>>());
            var _simpleMapper = new SimpleMapper();

            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfiles());
            });
            var mapper = mockMapper.CreateMapper();

            var serviceProviderMock = new Mock<IServiceProvider>();

            serviceProviderMock.Setup(x => x.GetService(typeof(Parser))).Returns(new Parser(mapper, _simpleMapper, _simpleValidator));

            _serviceProvider = serviceProviderMock.Object;

        }

        [Fact]
        public void Read_ShouldReturn_NotEmptyCollection()
        {
            //Arrange
            var filePath = @"C:\Projects\TradeLoader\tradedata\buy.txt";
            var tradeReader = TradeReader.FromFile(_serviceProvider, filePath);

            //Act
            var trades = tradeReader.Read();

            // Assert
            Assert.NotEmpty(trades);
        }
    }
}
