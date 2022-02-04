using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Transactions;
using TradeLoader.Interfaces;

namespace TradeLoader
{
    /// <summary>
    /// Stores trades in a database.
    /// </summary>
    public class StoreToDb : ITradeStore
    {
        private readonly ILogger _logger;

        public StoreToDb(ILogger<StoreToDb> logger)
        {
            _logger = logger;
        }

        public async Task<bool> Store(IEnumerable<Trade> trades, IConfiguration configuration)
        {
            if (trades == null)
                return false;

            string connectionString = configuration["ConnectionStrings:DefaultConnection"];
            try
            {
                using TransactionScope scope = new(TransactionScopeAsyncFlowOption.Enabled);
                using (SqlConnection connection = new(connectionString))
                {
                    await connection.OpenAsync();

                    foreach (var trade in trades)
                    {
                        var command = connection.CreateCommand();
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.CommandText = "dbo.AddTrade";
                        command.Parameters.AddWithValue("@TradeDate", trade.TradeDate);
                        command.Parameters.AddWithValue("@TradeType", trade.TradeType);
                        command.Parameters.AddWithValue("@BaseCurrency", trade.BaseCurrency);
                        command.Parameters.AddWithValue("@PriceCurrency", trade.PriceCurrency);
                        command.Parameters.AddWithValue("@Amount", trade.Amount);
                        command.Parameters.AddWithValue("@Price", trade.Price);

                        await command.ExecuteNonQueryAsync();
                    }
                }
                scope.Complete();
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Transaction is rolled back. {ex.Message}");
                return false;
            }

            _logger.LogInformation("Trades have been saved successfully.");

            return true;
        }
    }
}
