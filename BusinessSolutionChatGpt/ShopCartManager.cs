using BusinessSolutionChatGpt.DTO.Product;
using BusinessSolutionChatGpt.Interfaces;
using BusinessSolutionChatGpt.Services.Interfaces;

namespace BusinessSolutionChatGpt
{
    internal class ShopCartManager : IShopCartManager
    {
        private IAddProductService addProductService;
        private readonly IGetProductService getProductService;
        private readonly IDeleteProductService deleteProductService;

        public ShopCartManager(IAddProductService addProductService, IGetProductService getProductService, IDeleteProductService deleteProductService)
        {
            this.addProductService = addProductService;
            this.getProductService = getProductService;
            this.deleteProductService = deleteProductService;
        }

        void IShopCartManager.Add(AddProductDTO product) => addProductService.Add(product);

        List<ProductDetailsDTO> IShopCartManager.GetAll() => getProductService.GetAll();

        decimal IShopCartManager.GetTotalCost() => getProductService.GetPriceAll();

        void IShopCartManager.DeleteAll() => deleteProductService.DeleteAll();

        void IShopCartManager.Delete(int productId) => deleteProductService.Delete(productId);

        bool IShopCartManager.Exists(int productId) => getProductService.Exists(productId);
    }
}
