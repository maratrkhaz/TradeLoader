using Microsoft.Extensions.Configuration;
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
    /// Stores trades in a persistence.
    /// </summary>
    public class TradeSaver
    {
        private readonly ITradeStore _tradeStore;
        private readonly ILogger _logger;
        private readonly IConfigurationRoot _configuration;

        public TradeSaver(ITradeStore tradeStore, ILogger<TradeSaver> logger, IConfigurationRoot configuration)
        {
            _tradeStore = tradeStore;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<bool> ProcessStore(IEnumerable<Trade> trades)
        {
            if (trades == null)
            {
                _logger.LogWarning("List of trades is empty.");
                return false;
            }

            var result = await _tradeStore.Store(trades, _configuration);

            return result;
        }
        
    }
    
}
