using BusinessSolutionChatGpt;
using BusinessSolutionChatGpt.Commands.Interfaces;
using BusinessSolutionChatGpt.Infrastructure.Interfacrs;

namespace BusinessSolutionChatGpt.Commands
{
    internal class PrintTotalCostCommand : ICommand
    {
        private readonly IOutput output;
        private readonly ShopCartCalculator cartCalculator;

        public PrintTotalCostCommand(IOutput output, ShopCartCalculator cartCalculator)
        {
            this.output = output;
            this.cartCalculator = cartCalculator;
        }

        public void Execute()
        {
            output.WriteLineWithEscape($"tyle masz do zapłacenia: {cartCalculator.GetTotalCost()}");
        }
    }
}
