using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeLoader
{
    public class Trade
    {
        public int Id { get; set; }
        public DateTime TradeDate { get; set; }
        public string TradeType { get; set; }
        public string BaseCurrency { get; set; }
        public string PriceCurrency { get; set; }
        public int Amount { get; set; }
        public decimal Price { get; set; }
    }
}
