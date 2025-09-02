using BusinessSolutionChatGpt.DTO.Input;
using BusinessSolutionChatGpt.DTO.Product;
using BusinessSolutionChatGpt.Infrastructure.Interfaces;
using BusinessSolutionChatGpt.Interfaces;
using BusinessSolutionChatGpt.Validators;
using FluentValidation;
using log4net;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using System.Globalization;

namespace BusinessSolutionChatGpt
{
    internal class ShopApp : IShopApp
    {
        private readonly IOutput output;
        private readonly IInput input;
        private readonly IShopCartManager shopCartManager;
        private readonly IInputRetriever<string> productNameRetriever;
        private readonly IInputRetriever<decimal> productPriceRetriever;
        private readonly IInputRetriever<int> productIdentifierRetriever;
        private readonly IStringLocalizer<BusinessSolutionChatGpt.Resources.SharedResource> localizer;
        private readonly ILog log;
        private readonly ShopCartPrinter shopCartPrinter;

        public ShopApp(IOutput output,
            IInput input,
            IShopCartManager shopCartManager,
            ProductNameLoopDataRetriever productNameRetriever,
            ProductPriceLoopDataRetriever productPriceRetriever,
            ProductIdLoopDataRetriever productIdentifierRetriever,
            IStringLocalizer<BusinessSolutionChatGpt.Resources.SharedResource> localizer,
            ILog log) 
        {
            this.output = output;
            this.input = input;
            this.shopCartManager = shopCartManager;
            this.productNameRetriever = productNameRetriever;
            this.productPriceRetriever = productPriceRetriever;
            this.productIdentifierRetriever = productIdentifierRetriever;
            this.localizer = localizer;
            this.log = log;
            shopCartPrinter = new ShopCartPrinter(output, this.shopCartManager);
        }

        public void Start()
        {
            ConsoleKeyInfo readedKey;
            do
            {
                output.WriteLine(string.Empty);
                output.WriteLine(localizer["AddProductInstruction"]);
                output.WriteLine(localizer["ShowAllProductsInstruction"]);
                output.WriteLine(localizer["ShowTotalCostInstruction"]);
                output.WriteLine(localizer["RemoveSpecifiedProductInstruction"]);
                output.WriteLine(localizer["RemoveAllProductsInstruction"]);
                output.WriteLine(localizer["StopShopAppInstruction"]);

                readedKey = input.ReadKey();
                switch (readedKey.Key)
                {
                    case ConsoleKey.D1:
                        string productName = productNameRetriever.TryGet()!;
                        decimal productPrice = productPriceRetriever.TryGet();
                        var product = new AddProductDTO { Name = productName, Price = productPrice };
                        log.Debug($"Użytkownik stworzył produkt {JsonConvert.SerializeObject(product)}");
                        shopCartManager.Add(product);
                        break;
                    case ConsoleKey.D2:
                        log.Debug($"Użytkownik wyświetił wszystkie produkty");
                        shopCartPrinter.Print();
                        break;
                    case ConsoleKey.D3:
                        log.Debug($"Użytkownik wyświetił całkowity koszt");
                        output.WriteLineWithEscape($"Całkowity koszt to: {shopCartManager.GetTotalCost().ToString(CultureInfo.InvariantCulture)}");
                        break;
                    case ConsoleKey.D4:
                        var productId = productIdentifierRetriever.TryGet();
                        log.Debug($"Użytkownik próbuje produkt {productId}");
                        shopCartManager.Delete(productId);
                        output.WriteLine($"Usunięto produkt o identyfikatorze: {productId}");
                        break;
                    case ConsoleKey.D5:
                        log.Debug($"Użytkownik wyczyścił koszyk");
                        output.WriteLine("Koszyk został wyczyszczony");
                        shopCartManager.DeleteAll();
                        break;
                    default:
                        log.Debug($"Użytkownik wybrał niepoprawną komendę");
                        output.WriteLineWithEscape("niepoprawna komenda");
                        continue;
                }
            }
            while (readedKey.Key != ConsoleKey.Escape);
        }
    }
}
