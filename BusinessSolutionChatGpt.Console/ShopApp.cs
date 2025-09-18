using BusinessSolutionChatGpt.Console.Infrastructure.Interfaces;
using BusinessSolutionChatGpt.Console.Interfaces;
using BusinessSolutionChatGpt.Core.DTO.Product;
using BusinessSolutionChatGpt.Core.Interfaces;
using BusinessSolutionChatGpt.Core.Validators;
using FluentValidation;
using log4net;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using System.Globalization;

namespace BusinessSolutionChatGpt.Console
{
    internal class ShopApp : IShopApp
    {
        private readonly IOutput output;
        private readonly IInput input;
        private readonly IShopCartManager shopCartManager;
        private readonly IValidator<AddProductDTO> addProductValidator;
        private readonly ProductExistValidator productExistValidator;
        private readonly IStringLocalizer localizer;
        private readonly ILog log;
        private readonly ShopCartPrinter shopCartPrinter;

        public ShopApp(IOutput output,
            IInput input,
            IShopCartManager shopCartManager,
            IValidator<AddProductDTO> addProductValidator,
            ProductExistValidator productExistValidator,
            IStringLocalizer localizer,
            ILog log) 
        {
            this.output = output;
            this.input = input;
            this.shopCartManager = shopCartManager;
            this.addProductValidator = addProductValidator;
            this.productExistValidator = productExistValidator;
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
                output.WriteLine(localizer.GetString("AddProductInstruction").Value);
                output.WriteLine(localizer.GetString("ShowAllProductsInstruction").Value);
                output.WriteLine(localizer.GetString("ShowTotalCostInstruction").Value);
                output.WriteLine(localizer.GetString("RemoveSpecifiedProductInstruction").Value);
                output.WriteLine(localizer.GetString("RemoveAllProductsInstruction").Value);
                output.WriteLine(localizer.GetString("StopShopAppInstruction").Value);

                readedKey = input.ReadKey();
                switch (readedKey.Key)
                {
                    case ConsoleKey.D1:
                        var product = input.ReadObject(addProductValidator, output);
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
                        var productId = input.ReadPrimitive(productExistValidator, output, localizer["ProductIdentifierInstruction"]);
                        log.Debug($"Użytkownik próbuje produkt {productId}");
                        shopCartManager.Delete(productId - 1);
                        output.WriteLine($"Usunięto produkt o identyfikatorze: {productId}");
                        break;
                    case ConsoleKey.D5:
                        log.Debug($"Użytkownik wyczyścił koszyk");
                        output.WriteLine("Koszyk został wyczyszczony");
                        shopCartManager.DeleteAll();
                        break;
                    case ConsoleKey.Escape:
                        log.Debug($"Użytkownik zakończył pracę");
                        continue;
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
