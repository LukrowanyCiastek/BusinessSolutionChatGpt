using BusinessSolutionChatGpt.DTO.Product;
using BusinessSolutionChatGpt.Interfaces;
using BusinessSolutionChatGpt.Model;
using BusinessSolutionChatGpt.Services.Interfaces;

namespace BusinessSolutionChatGpt
{
    internal class ShopCartManager : IShopCartManager
    {
        private IAddProductService addProductService;
        private readonly IGetProductService getProductService;

        public ShopCartManager(IAddProductService addProductService, IGetProductService getProductService)
        {
            this.addProductService = addProductService;
            this.getProductService = getProductService;
        }

        void IShopCartManager.Add(AddProductDTO product) => addProductService.Add(product);

        List<ProductDetailsDTO> IShopCartManager.GetAll() => getProductService.GetAll();

        public decimal GetTotalCost() => getProductService.GetPriceAll();
    }
}
