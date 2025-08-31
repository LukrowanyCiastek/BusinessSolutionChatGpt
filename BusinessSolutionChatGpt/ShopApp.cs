using BusinessSolutionChatGpt.DTO.Product;
using BusinessSolutionChatGpt.Infrastructure.Interfaces;
using BusinessSolutionChatGpt.Interfaces;
using BusinessSolutionChatGpt.Parsers.Interfaces;
using BusinessSolutionChatGpt.Validators;
using BusinessSolutionChatGpt.Validators.Interfaces;
using log4net;
using Newtonsoft.Json;

namespace BusinessSolutionChatGpt
{
    internal class ShopApp : IShopApp
    {
        private readonly IOutput output;
        private readonly IInput input;
        private readonly IShopCartManager shopCartManager;
        private readonly IParser<string> stringParser;
        private readonly IParser<decimal> decimalParser;
        private readonly IParser<int> intParser;
        private readonly ILog log;
        private readonly ShopCartPrinter shopCartPrinter;

        public ShopApp(IOutput output,
            IInput input,
            IShopCartManager shopCartManager,
            IParser<string> stringParser,
            IParser<decimal> decimalParser,
            IParser<int> intParser,
            ILog log) 
        {
            this.output = output;
            this.input = input;
            this.shopCartManager = shopCartManager;
            this.stringParser = stringParser;
            this.decimalParser = decimalParser;
            this.intParser = intParser;
            this.log = log;
            shopCartPrinter = new ShopCartPrinter(output, this.shopCartManager);
        }

        public void Start()
        {
            ConsoleKeyInfo readedKey;

            IValidator<string> stringValidator = new NotNullOrEmptyStringValidator();
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
                        IInputRetriever<string> productNameRetriever = new LoopDataRetriever<string>(output, input, stringValidator, stringValidator, stringParser, "Podaj nazwę produktu", "Nazwa niepoprawna spróbuj ponownie");
                        string productName = productNameRetriever.TryGet();

                        IValidator<string> decimalValidator = new PositiveDecimalValidator(decimalParser);
                        IInputRetriever<decimal> priceProductRetriever = new LoopDataRetriever<decimal>(output, input, stringValidator, decimalValidator, decimalParser, "Podaj cenę produktu", "Cena niepoprawna spróbuj ponownie");
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
                        output.WriteLine($"Całkowity koszt to: {shopCartManager.GetTotalCost()}");
                        break;
                    case ConsoleKey.D4:
                        IValidator<string> productValidator = new ProductIdValidator(intParser, shopCartManager);
                        IInputRetriever<int> indexRetriever = new LoopDataRetriever<int>(output, input, stringValidator, productValidator, intParser, "Podaj identyfikator produktu", "Produkt nie istnieje");
                        var productId = indexRetriever.TryGet();
                        log.Debug($"Użytkownik próbuje produkt {productId}");
                        shopCartManager.Delete(productId);
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
