using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeLoader.Interfaces;

namespace TradeLoader
{
    /// <summary>
    /// Validates trade data array and logs warnings
    /// </summary>
    public class SimpleValidator : ITradeValidator
    {
        private readonly ILogger _logger;

        public SimpleValidator(ILogger<SimpleValidator> logger)
        {
            _logger = logger;
        }

        public bool Validate(string[] trade)
        {
            if (trade.Length != 5)
            {
                _logger.LogWarning($"There are {trade.Length} field(s) found. But 5 fields are expected. Trade Data {String.Join(";",trade)}");
                return false;
            }

            if (trade[0].Length != 6)
            {
                _logger.LogWarning($"Trade currencies {trade[0]} are not valid. Trade Data {String.Join(";", trade)}");
                return false;
            }

            if (!int.TryParse(trade[1], out _))
            {
                _logger.LogWarning($"Trade amount is not a valid integer: '{trade[1]}'. Trade Data {String.Join(";", trade)}");
                return false;
            }

            if (!decimal.TryParse(trade[2], out _))
            {
                _logger.LogWarning($"Trade price is not a valid decimal: '{trade[2]}'. Trade Data {String.Join(";", trade)}");
                return false;
            }

            if (!DateTime.TryParse(trade[4], out _))
            {
                _logger.LogWarning($"Trade date is not a valid DateTime: '{trade[4]}'. Trade Data {String.Join(";", trade)}");
                return false;
            }

            return true;
        }
    }
}
