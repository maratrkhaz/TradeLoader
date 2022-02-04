using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeLoader.Interfaces;

namespace TradeLoader
{
    /// <summary>
    /// Reads and parses trades from file.
    /// </summary>
    public class TradeReader : IDisposable
    {
        private readonly Stream _stream;
        private readonly ITradeParser _parser;
        private TradeReader(Stream stream, ITradeParser parser)
        {
            _stream = stream;
            _parser = parser;
        }
        /// <summary>
        /// Factory method creates disposable TradeReader type with filestream and trades parser
        /// </summary>
        /// <param name="services">Inversion of control container</param>
        /// <param name="filePath">File path with trade data</param>
        /// <returns></returns>
        public static TradeReader FromFile(IServiceProvider services, string filePath)
        {
            var fs = new FileStream(filePath, FileMode.Open);
            var parser = services.GetRequiredService<Parser>();

            return new TradeReader(fs, parser);
        }

        public void Dispose()
        {
            _stream.Close();
        }

        /// <summary>
        /// Reads and parses file.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Trade>> Read()
        {
            using var sr = new StreamReader(_stream);
            string line = null;
            var trades = new List<Trade>();
            while ((line = await sr.ReadLineAsync()) != null)
            {
                if (_parser.Parse(line, out Trade tradeData))
                {
                    trades.Add(tradeData);
                }
            }

            return trades;
        }

    }
}
