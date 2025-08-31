using BusinessSolutionChatGpt;
using BusinessSolutionChatGpt.Commands.Interfaces;
using BusinessSolutionChatGpt.Infrastructure.Interfacrs;

namespace BusinessSolutionChatGpt.Commands
{
    internal class PrintAllProductCommand : ICommand
    {
        private readonly IOutput output;
        private readonly ShopCartPrinter shopCartPrinter;

        public PrintAllProductCommand(IOutput output, ShopCartPrinter shopCartPrinter)
        {
            this.output = output;
            this.shopCartPrinter = shopCartPrinter;
        }

        public void Execute()
        {
            output.WriteLineWithEscape("Oto wszystkie produkty");
            shopCartPrinter.Print();
        }
    }
}
