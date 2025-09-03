
using BusinessSolutionChatGpt.Core;
using BusinessSolutionChatGpt.Core.Interfaces;
using BusinessSolutionChatGpt.Core.Model;
using BusinessSolutionChatGpt.Core.Services;
using BusinessSolutionChatGpt.Core.Services.Interfaces;
using BusinessSolutionChatGpt.Core.Validators;
using BusinessSolutionChatGpt.Services;
using BusinessSolutionChatGpt.Validators;
using log4net;
using log4net.Config;
using log4net.Repository;
using Microsoft.Extensions.Localization;
using System.Reflection;

namespace BusinessSolutionChatGpt.Api
{

    public class Program
    {

        private static readonly ILog log = LogManager.GetLogger(typeof(Program));

        public static void Main(string[] args)
        {
            log.Info("Aplikacja rozpoczê³a pracê");
            ILoggerRepository logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly()!);
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

            var builder = WebApplication.CreateBuilder(args);

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
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
            serviceCollection.AddSingleton<IList<Product>>(new List<Product>());
            serviceCollection.AddSingleton<IProductRepository, ProductRepository>();
            serviceCollection.AddSingleton<IAddProductService, AddProductService>();
            serviceCollection.AddSingleton<IGetProductService, GetProductService>();
            serviceCollection.AddSingleton<IDeleteProductService, DeleteProductService>();
            serviceCollection.AddScoped<IShopCartManager, ShopCartManager>();
            serviceCollection.AddTransient<AddProductValidator>();
            serviceCollection.AddTransient<ProductExistValidator>();
        }
    }
}
