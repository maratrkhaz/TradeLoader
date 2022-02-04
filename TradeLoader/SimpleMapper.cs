using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeLoader.Dtos;
using TradeLoader.Interfaces;
using TradeLoader.Profiles;

namespace TradeLoader
{
    /// <summary>
    /// Maps trade data array to Trade type
    /// </summary>
    public class SimpleMapper : ITradeMapper
    {
        public TradeDto Map(string[] fields)
        {
            var baseCurrency = fields[0].Substring(0, 3);
            var priceCurrency = fields[0].Substring(3, 3);
            var tradeAmount = fields[1];
            var tradePrice = fields[2];
            var tradeType = fields[3];
            var tradeDate = fields[4];

            var tradeDto = new TradeDto
            {
                BaseCurrency = baseCurrency,
                PriceCurrency = priceCurrency,
                Amount = tradeAmount,
                Price = tradePrice,
                TradeType = tradeType,
                TradeDate = tradeDate
            };

            return tradeDto;
        }

        
    }
}
