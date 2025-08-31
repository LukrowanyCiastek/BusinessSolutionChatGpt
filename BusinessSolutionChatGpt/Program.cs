using BusinessSolutionChatGpt.Infrastructure;
using BusinessSolutionChatGpt.Infrastructure.Interfacrs;
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

            serviceCollection.AddScoped<IAddProductService, AddProductSercvice>();
            serviceCollection.AddScoped<IShopCartManager, ShopCartManager>();
            serviceCollection.AddScoped<IShopApp, ShopApp>();            
        }
    }
}
