using BusinessSolutionChatGpt.Core.DTO.Product;
using BusinessSolutionChatGpt.Core.Interfaces;
using BusinessSolutionChatGpt.Core.Services.Interfaces;

namespace BusinessSolutionChatGpt.Core
{
    public class ShopCartManager : IShopCartManager
    {
        private readonly IAddProductService addProductService;
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

        decimal IShopCartManager.GetTotalCost() => getProductService.GetTotalCost();

        void IShopCartManager.DeleteAll() => deleteProductService.DeleteAll();

        void IShopCartManager.Delete(long productId) => deleteProductService.Delete(productId);

        bool IShopCartManager.Exists(long productId) => getProductService.Exists(productId);
    }
}
