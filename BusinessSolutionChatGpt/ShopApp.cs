using BusinessSolutionChatGpt.Commands;
using BusinessSolutionChatGpt.Commands.Interfaces;
using BusinessSolutionChatGpt.Infrastructure.Interfacrs;
using BusinessSolutionChatGpt.Model;
using BusinessSolutionChatGpt.Parsers;
using BusinessSolutionChatGpt.Parsers.Interfaces;
using BusinessSolutionChatGpt.Services.Interfaces;
using BusinessSolutionChatGpt.Validators;
using BusinessSolutionChatGpt.Validators.Interfaces;

namespace BusinessSolutionChatGpt
{
    internal class ShopApp : IShopApp
    {
        private readonly IOutput output;
        private readonly IInput input;
        private readonly IAddProductService addProductService;
        private readonly ShopCartPrinter shopCartPrinter;
        private readonly ShopCartCalculator shopCartCalculator;

        public ShopApp(IOutput output, IInput input, IAddProductService addProductService, IShopCartManager shopCartManager) 
        {
            this.output = output;
            this.input = input;
            this.addProductService = addProductService;
            shopCartPrinter = new ShopCartPrinter(output, shopCartManager);
            shopCartCalculator = new ShopCartCalculator(shopCartManager);
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
                ICommand command;
                var shopCartManager = new ShopCartManager();
                switch (readedKey.Key)
                {
                    case ConsoleKey.D1:
                        IInputRetriever<string> productNameRetriever = new LoopDataRetriever<string>(output, input, stringValidator, stringValidator, stringParser, "Podaj nazwę produktu", "Nazwa niepoprawna spróbuj ponownie");
                        string productName = productNameRetriever.TryGet();

                        IParser<decimal> decimalParser = new DecimalParser();
                        IValidator<string> decimalValidator = new PositiveDecimalValidator(decimalParser);
                        IInputRetriever<decimal> priceProductRetriever = new LoopDataRetriever<decimal>(output, input, stringValidator, decimalValidator, decimalParser, "Podaj cenę produktu", "Cena niepoprawna spróbuj ponownie");
                        decimal productPrice = priceProductRetriever.TryGet();

                        command = new AddProductCommand(addProductService, new Product { Name = productName!, Price = productPrice });
                        command.Execute();
                        break;
                    case ConsoleKey.D2:

                        command = new PrintAllProductCommand(output, shopCartPrinter);
                        command.Execute();
                        break;
                    case ConsoleKey.D3:
                        output.WriteLineWithEscape("Oto wszystkie produkty");
                        command = new PrintTotalCostCommand(output, shopCartCalculator);
                        command.Execute();
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
