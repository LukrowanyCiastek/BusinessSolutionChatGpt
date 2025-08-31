using BusinessSolutionChatGpt.DTO.Product;

namespace BusinessSolutionChatGpt.Interfaces
{
    internal interface IShopCartManager
    {
        void Add(AddProductDTO product);

        List<ProductDetailsDTO> GetAll();

        decimal GetTotalCost();
    }
}
