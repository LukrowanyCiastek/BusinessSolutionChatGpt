using BusinessSolutionChatGpt.DTO.Input;
using BusinessSolutionChatGpt.DTO.Product;
using BusinessSolutionChatGpt.Infrastructure.Interfaces;
using BusinessSolutionChatGpt.Interfaces;
using BusinessSolutionChatGpt.Validators;
using FluentValidation;
using log4net;
using Newtonsoft.Json;
using System.Globalization;

namespace BusinessSolutionChatGpt
{
    internal class ShopApp : IShopApp
    {
        private readonly IOutput output;
        private readonly IInput input;
        private readonly IShopCartManager shopCartManager;
        private readonly ILog log;
        private readonly ShopCartPrinter shopCartPrinter;

        public ShopApp(IOutput output,
            IInput input,
            IShopCartManager shopCartManager,
            ILog log) 
        {
            this.output = output;
            this.input = input;
            this.shopCartManager = shopCartManager;
            this.log = log;
            shopCartPrinter = new ShopCartPrinter(output, this.shopCartManager);
        }

        public void Start()
        {
            ConsoleKeyInfo readedKey;
            do
            {
                output.WriteLine(string.Empty);
                output.WriteLine("Naciśnij 1 aby rozpocząć dodawanie produktu do koszyka");
                output.WriteLine("Naciśnij 2 aby rozpocząć wyświetlić wszystkie produkty w koszyku");
                output.WriteLine("Naciśnij 3 aby rozpocząć zobaczyć ile masz do zapłacenia");
                output.WriteLine("Naciśnij 4 aby usnąć konkretny produkt");
                output.WriteLine("Naciśnij 5 aby usunąć wszystkie książki");
                output.WriteLine("Naciśnij Esc aby zakończyć pracę");

                readedKey = input.ReadKey();
                switch (readedKey.Key)
                {
                    case ConsoleKey.D1:
                        IInputRetriever<string> productNameRetriever = new LoopDataRetriever<string>(output, input, new FluentNotNullOrEmptyStringValidator("Nazwa została nie podana", "Nazwa jest pusta"), "Podaj nazwę produktu");
                        string productName = productNameRetriever.TryGet()!;

                        IInputRetriever<decimal> priceProductRetriever = new LoopDataRetriever<decimal>(output, input, new FluentPositiveDecimalValidator("Cena nie została podana", "Cena jest psuta", "Ciąg znaków to nie cena", "Cena nie może być mniejsza od 0"), "Podaj cenę produktu");
                        decimal productPrice = priceProductRetriever.TryGet();
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
                        AbstractValidator<InputDTO<int>> productValidator = new FluentProductIdValidator("Podany ciąg znaków jest null", "Podany ciąg znaków jest pusty", "Niepoprawny identyfikator", "Produkt nie istnieje", shopCartManager);
                        IInputRetriever<int> indexRetriever = new LoopDataRetriever<int>(output, input, productValidator, "Podaj identyfikator produktu");
                        var productId = indexRetriever.TryGet();
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
