using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeLoader.Interfaces
{
    public interface ITradeStore
    {
        Task<bool> Store(IEnumerable<Trade> trades, IConfiguration configuration);
    }
}
