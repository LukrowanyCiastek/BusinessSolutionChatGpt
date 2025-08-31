using BusinessSolutionChatGpt.Commands.Interfaces;
using BusinessSolutionChatGpt.Model;
using BusinessSolutionChatGpt.Services.Interfaces;

namespace BusinessSolutionChatGpt.Commands
{
    internal class AddProductCommand : ICommand
    {
        private readonly IAddProductService addProductService;
        private readonly Product product;

        public AddProductCommand(IAddProductService addProductService, Product product)
        {
            this.addProductService = addProductService;
            this.product = product;
        }

        public void Execute()
        {
            addProductService.Add(product);
        }
    }
}
