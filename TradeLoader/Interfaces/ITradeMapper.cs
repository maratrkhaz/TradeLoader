using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeLoader.Dtos;

namespace TradeLoader.Interfaces
{
    public interface ITradeMapper
    {
        TradeDto Map(string[] fields);
    }
}
