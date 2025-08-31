using BusinessSolutionChatGpt.Infrastructure;
using BusinessSolutionChatGpt.Infrastructure.Interfaces;
using BusinessSolutionChatGpt.Interfaces;
using BusinessSolutionChatGpt.Model;
using BusinessSolutionChatGpt.Parsers;
using BusinessSolutionChatGpt.Parsers.Interfaces;
using BusinessSolutionChatGpt.Services;
using BusinessSolutionChatGpt.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessSolutionChatGpt
{
    internal class Program
    {
        static int Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            
            var serviceProvider = serviceCollection.BuildServiceProvider();

            var output = serviceProvider.GetRequiredService<IOutput>();
            var manager = serviceProvider.GetRequiredService<IShopApp>();
            manager.Start();

            output.WriteLineWithEscape("Program zakończył działanie");
            return 0;
        }

        private static void ConfigureServices(ServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IOutput, ConsoleOutput>();
            serviceCollection.AddSingleton<IInput, ConsoleInput>();
            serviceCollection.AddTransient<IParser<string>, StringParser>();
            serviceCollection.AddTransient<IParser<decimal>, DecimalParser>();
            serviceCollection.AddSingleton<IList<Product>>(new List<Product>());
            serviceCollection.AddSingleton<IAddProductService, AddProductService>();
            serviceCollection.AddSingleton<IGetProductService, GetProductService>();
            serviceCollection.AddSingleton<IShopCartManager, ShopCartManager>();
            serviceCollection.AddSingleton<IShopApp, ShopApp>();            
        }
    }
}
