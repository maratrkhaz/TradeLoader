using TradeLoader;
using TradeLoader.Dtos;
using Xunit;
using FluentAssertions;

namespace TradeLoaderTests
{
    public class SimpleMapperTests
    {
        private readonly SimpleMapper _simpleMapper;

        public SimpleMapperTests()
        {
            _simpleMapper = new SimpleMapper();
        }

        [Fact]
        public void Map_ShouldReturn_TradeDto()
        {
            //Arrange
            var trade = new string[] { "GBPUSD", "1000", "1,3553", "buy", "2022.01.01 10:00:00" };

            var tradeDtoExpected = new TradeDto
            { 
                BaseCurrency = "GBP",
                PriceCurrency = "USD", 
                Amount = "1000",
                Price = "1,3553",
                TradeType = "buy",
                TradeDate = "2022.01.01 10:00:00"
            };

            //Act
            var tradeDtoActual = _simpleMapper.Map(trade);

            //Assert
            tradeDtoExpected.Should().BeEquivalentTo(tradeDtoActual);
        }

    }
}
