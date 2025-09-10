using BusinessSolutionChatGpt.Console.Infrastructure;
using BusinessSolutionChatGpt.Console.Infrastructure.Interfaces;
using BusinessSolutionChatGpt.Console.Interfaces;
using BusinessSolutionChatGpt.Core;
using BusinessSolutionChatGpt.Core.Interfaces;
using BusinessSolutionChatGpt.Core.Model;
using BusinessSolutionChatGpt.Core.Services;
using BusinessSolutionChatGpt.Core.Services.Interfaces;
using BusinessSolutionChatGpt.Core.Validators;
using BusinessSolutionChatGpt.Services;
using log4net;
using log4net.Config;
using log4net.Repository;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Reflection;

namespace BusinessSolutionChatGpt.Console
{
    internal class Program
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Program));

        static int Main(string[] args)
        {
            log.Info("Aplikacja rozpoczęła pracę");
            ILoggerRepository logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly()!);
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

            try
            {
                var serviceCollection = new ServiceCollection();
                ConfigureServices(serviceCollection);

                var serviceProvider = serviceCollection.BuildServiceProvider();

                CultureInfo.CurrentUICulture = new CultureInfo("pl");
                var output = serviceProvider.GetRequiredService<IOutput>();
                var manager = serviceProvider.GetRequiredService<IShopApp>();
                manager.Start();

                output.WriteLineWithEscape("Program zakończył działanie");

                return 0;
            }
            catch (Exception ex)
            {
                log.Error("Program przerwał pracę przez błąd.", ex);
                return -1;
            }
        }

        private static void ConfigureServices(ServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton(log);
            serviceCollection.AddLogging(builder =>
            {
                builder.AddConsole();
                builder.SetMinimumLevel(LogLevel.Warning);
            });
            serviceCollection.AddLocalization(options => options.ResourcesPath = "Resources");
            serviceCollection.AddSingleton(x => x.GetRequiredService<IStringLocalizerFactory>().Create("SharedResource", typeof(Program).Assembly.GetName().Name!));
            serviceCollection.AddSingleton<IOutput, ConsoleOutput>();
            serviceCollection.AddSingleton<IInput, ConsoleInput>();
            serviceCollection.AddSingleton<IList<Product>>(new List<Product>());
            serviceCollection.AddSingleton<IProductRepository, ProductRepository>();
            serviceCollection.AddSingleton<IAddProductService, AddProductService>();
            serviceCollection.AddSingleton<IGetProductService, GetProductService>();
            serviceCollection.AddSingleton<IDeleteProductService, DeleteProductService>();
            serviceCollection.AddScoped<IShopCartManager, ShopCartManager>();
            serviceCollection.AddScoped<AddProductValidator>();
            serviceCollection.AddScoped<ProductExistValidator>();
            serviceCollection.AddSingleton<IShopApp, ShopApp>();
        }
    }
}
