using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeLoader;
using Xunit;

namespace TradeLoaderTests
{
    public class SimpleValidatorTests
    {
        private readonly SimpleValidator _simpleValidator;

        public SimpleValidatorTests()
        {
            _simpleValidator = new SimpleValidator(Mock.Of<ILogger<SimpleValidator>>());
        }

        [Fact]
        public void Validate_ShouldReturn_True()
        {
            //Arrange
            var trade = new string[] { "GBPUSD", "1000", "1,3553", "buy", "2022.01.01 10:00:00" };

            //Act
            var result = _simpleValidator.Validate(trade);

            //Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData("GBPUSD", "1000", "1,3553", "buy")]
        [InlineData("GBPUS", "1000", "1,3553", "buy", "2022.01.01 10:00:00")]
        [InlineData("GBPUSD_", "1000", "1,3553", "buy", "2022.01.01 10:00:00")]
        [InlineData("GBPUSD", "1000,1", "1,3553", "buy", "2022.01.01 10:00:00")]
        [InlineData("GBPUSD", "1000,1", "1b3553", "buy", "2022.01.01 10:00:00")]
        [InlineData("GBPUSD", "1000,1", "1,3553", "buy", "2022.01 10:00:00")]
        public void Validate_ShouldReturn_False(params string[] tradeData)
        {
            //Act
            var result = _simpleValidator.Validate(tradeData);

            //Assert
            Assert.False(result);
        }
    }
}


