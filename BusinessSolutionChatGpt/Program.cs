using BusinessSolutionChatGpt.DTO.Input;
using BusinessSolutionChatGpt.Infrastructure;
using BusinessSolutionChatGpt.Infrastructure.Interfaces;
using BusinessSolutionChatGpt.Interfaces;
using BusinessSolutionChatGpt.Model;
using BusinessSolutionChatGpt.Services;
using BusinessSolutionChatGpt.Services.Interfaces;
using BusinessSolutionChatGpt.Validators;
using FluentValidation;
using log4net;
using log4net.Config;
using log4net.Repository;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BusinessSolutionChatGpt
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
            serviceCollection.AddSingleton<IOutput, ConsoleOutput>();
            serviceCollection.AddSingleton<IInput, ConsoleInput>();
            serviceCollection.AddSingleton<IList<Product>>(new List<Product>());
            serviceCollection.AddSingleton<IProductRepository, ProductRepository>();
            serviceCollection.AddSingleton<IAddProductService, AddProductService>();
            serviceCollection.AddSingleton<IGetProductService, GetProductService>();
            serviceCollection.AddSingleton<IDeleteProductService, DeleteProductService>();
            serviceCollection.AddSingleton<IShopCartManager, ShopCartManager>();
            serviceCollection.AddSingleton(new NotNullOrEmptyStringValidator(Resources.Resources.ProductMissingNameValidationMessage, Resources.Resources.ProductEmptyNameValidationMessage));
            serviceCollection.AddSingleton(
                new PositiveDecimalValidator(
                    Resources.Resources.ProductMissingPriceValidationMessage,
                    Resources.Resources.ProductEmptyPriceValidationMessage,
                    Resources.Resources.ProductNotDecimalPriceValidationMessage,
                    Resources.Resources.ProductNotPositivePriceValidationMessage
                ));
            serviceCollection.AddSingleton(x =>
                new ProductIdValidator(
                    Resources.Resources.ProductMissingIdentifierValidationMessage,
                    Resources.Resources.ProductEmptyIdentifierValidationMessage,
                    Resources.Resources.ProductNotIntegerIdentifierValidationMessage,
                    Resources.Resources.ProductNotExistValidationMessage,
                    x.GetService<IShopCartManager>()!));
            serviceCollection.AddSingleton<IInputRetriever<string>>(x => new LoopDataRetriever<string>(x.GetService<IOutput>()!, x.GetService<IInput>()!, x.GetService<NotNullOrEmptyStringValidator>()!, Resources.Resources.ProductNameInstruction));
            serviceCollection.AddSingleton<IInputRetriever<decimal>>(x => new LoopDataRetriever<decimal>(x.GetService<IOutput>()!, x.GetService<IInput>()!, x.GetService<PositiveDecimalValidator>()!, Resources.Resources.ProductPriceInstruction));
            serviceCollection.AddSingleton<IInputRetriever<int>>(x => new LoopDataRetriever<int>(x.GetService<IOutput>()!, x.GetService<IInput>()!, x.GetService<ProductIdValidator>()!, Resources.Resources.ProductIdentifierInstruction));
            serviceCollection.AddSingleton<IShopApp, ShopApp>();
        }
    }
}
