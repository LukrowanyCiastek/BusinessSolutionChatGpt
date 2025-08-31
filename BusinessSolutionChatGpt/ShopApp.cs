using BusinessSolutionChatGpt.DTO.Product;
using BusinessSolutionChatGpt.Infrastructure.Interfaces;
using BusinessSolutionChatGpt.Interfaces;
using BusinessSolutionChatGpt.Parsers;
using BusinessSolutionChatGpt.Parsers.Interfaces;
using BusinessSolutionChatGpt.Validators;
using BusinessSolutionChatGpt.Validators.Interfaces;

namespace BusinessSolutionChatGpt
{
    internal class ShopApp : IShopApp
    {
        private readonly IOutput output;
        private readonly IInput input;
        private readonly IShopCartManager shopCartManager;
        private readonly ShopCartPrinter shopCartPrinter;

        public ShopApp(IOutput output, IInput input,  IShopCartManager shopCartManager) 
        {
            this.output = output;
            this.input = input;
            this.shopCartManager = shopCartManager;
            shopCartPrinter = new ShopCartPrinter(output, this.shopCartManager);
        }

        public void Start()
        {
            IParser<string> stringParser = new StringParser();

            ConsoleKeyInfo readedKey;

            IValidator<string> stringValidator = new NotNullOrEmptyStringValidator();
            do
            {
                output.WriteLine(string.Empty);
                output.WriteLine("Naciśnij 1 aby rozpocząć dodawanie produktu do koszyka");
                output.WriteLine("Naciśnij 2 aby rozpocząć wyświetlić wszystkie produkty w koszyku");
                output.WriteLine("Naciśnij 3 aby rozpocząć zobaczyć ile masz do zapłacenia");
                output.WriteLine("Naciśnij Esc aby zakończyć pracę");

                readedKey = input.ReadKey();
                switch (readedKey.Key)
                {
                    case ConsoleKey.D1:
                        IInputRetriever<string> productNameRetriever = new LoopDataRetriever<string>(output, input, stringValidator, stringValidator, stringParser, "Podaj nazwę produktu", "Nazwa niepoprawna spróbuj ponownie");
                        string productName = productNameRetriever.TryGet();

                        IParser<decimal> decimalParser = new DecimalParser();
                        IValidator<string> decimalValidator = new PositiveDecimalValidator(decimalParser);
                        IInputRetriever<decimal> priceProductRetriever = new LoopDataRetriever<decimal>(output, input, stringValidator, decimalValidator, decimalParser, "Podaj cenę produktu", "Cena niepoprawna spróbuj ponownie");
                        decimal productPrice = priceProductRetriever.TryGet();

                        shopCartManager.Add(new AddProductDTO { Name = productName, Price = productPrice });
                        break;
                    case ConsoleKey.D2:

                        shopCartPrinter.Print();
                        break;
                    case ConsoleKey.D3:
                        output.WriteLineWithEscape("Oto wszystkie produkty");
                        output.WriteLine($"Całkowity koszt to: {shopCartManager.GetTotalCost()}");
                        break;
                    default:
                        output.WriteLineWithEscape("niepoprawna komenda");
                        continue;
                }
            }
            while (readedKey.Key != ConsoleKey.Escape);
        }
    }
}
