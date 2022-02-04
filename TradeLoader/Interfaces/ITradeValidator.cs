using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeLoader.Interfaces
{
    public interface ITradeValidator
    {
        bool Validate(string[] trade);
    }
}
