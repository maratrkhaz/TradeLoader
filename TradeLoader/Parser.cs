using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeLoader.Dtos;
using TradeLoader.Interfaces;

namespace TradeLoader
{
    /// <summary>
    /// Parses string with trade data.
    /// </summary>
    public class Parser : ITradeParser
    {
        private readonly ITradeMapper _tradeMapper;
        private readonly ITradeValidator _tradeValidator;
        private readonly char _parseSign;
        private readonly IMapper _autoMapper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="autoMapper">Automapper interface</param>
        /// <param name="tradeMapper">Maps string array of trade data to Trade type</param>
        /// <param name="tradeValidator">Validate string array of trade data</param>
        /// <param name="parseSign">String delimiter of trade data to array</param>
        public Parser(IMapper autoMapper, ITradeMapper tradeMapper, ITradeValidator tradeValidator, char parseSign=';')
        {
            _tradeMapper = tradeMapper;
            _tradeValidator = tradeValidator;
            _parseSign = parseSign;
            _autoMapper = autoMapper;
        }

        public bool Parse(string tradeData, out Trade trade)
        {
            string[] fields = tradeData.Split(new char[] { _parseSign });

            if (!_tradeValidator.Validate(fields))
            {
                trade = null;
                return false;
            }

            var tradeDto = _tradeMapper.Map(fields);
            trade = _autoMapper.Map<TradeDto, Trade>(tradeDto);

            return true;
        }
    }
}
