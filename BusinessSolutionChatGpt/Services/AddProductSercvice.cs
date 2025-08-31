using BusinessSolutionChatGpt;
using BusinessSolutionChatGpt.Model;
using BusinessSolutionChatGpt.Services.Interfaces;

namespace BusinessSolutionChatGpt.Services
{
    internal class AddProductSercvice : IAddProductService
    {
        private readonly IShopCartManager shopCartManager;

        public AddProductSercvice(IShopCartManager database)
        { 
            shopCartManager = database;
        }

        void IAddProductService.Add(Product product)
        {
            shopCartManager.Add(product);
        }
    }
}
