using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TradeLoader.Dtos;
using TradeLoader.Interfaces;
using TradeLoader.Profiles;

namespace TradeLoader
{
    class Program
    {
        public static IConfigurationRoot configurationRoot;

        static void Main(string[] args)
        {
            //Serilog
            Log.Logger = new LoggerConfiguration()
            .WriteTo.Console(Serilog.Events.LogEventLevel.Debug)
            .MinimumLevel.Debug()
            .Enrich.FromLogContext()
            .CreateLogger();

            MainAsync(args).GetAwaiter().GetResult();
        }

        static async Task MainAsync(string[] args)
        {
            using IHost host = Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostingContext, configuration) =>
            {
                configuration 
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .Build();

                configurationRoot = configuration.Build();
                HandlingOptions options = new ();
                configurationRoot.GetSection("ConnectionStrings").Bind(options);
            })
             .ConfigureServices((_, services) =>
                    services.AddAutoMapper(typeof(MappingProfiles))
                            .AddSingleton(LoggerFactory.Create(builder =>
                            {
                                builder.AddSerilog(dispose: true);
                            }))
                            .AddLogging()
                            .AddSingleton<IConfigurationRoot>(configurationRoot)
                            .AddScoped<ITradeMapper, SimpleMapper>()
                            .AddScoped<ITradeValidator, SimpleValidator>()
                            .AddScoped<ITradeStore, StoreToDb>()
                            .AddScoped<Parser>()
                            .AddScoped<TradeSaver>())
            .Build();

            var tradesList = new BlockingCollection<Trade>();
            string[] files = Directory.GetFiles(@"C:\Projects\TradeLoader\tradedata");
            var tradeSaver = host.Services.GetRequiredService<TradeSaver>();

            Parallel.ForEach(files, async f =>
            {
                try
                {
                    using var reader = TradeReader.FromFile(host.Services, f);
                    var trades = await reader.Read();

                    foreach (var item in trades)
                    {
                        tradesList.Add(item);
                    }
                    
                }
                finally
                {
                    tradesList.CompleteAdding();
                }
            });

            var result = await tradeSaver.ProcessStore(tradesList);
            Console.WriteLine($"Result {result}");

            await host.RunAsync();
        }
    }
}
