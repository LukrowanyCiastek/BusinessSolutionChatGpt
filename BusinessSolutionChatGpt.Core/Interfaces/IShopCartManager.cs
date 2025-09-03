using BusinessSolutionChatGpt.Core.DTO.Product;

namespace BusinessSolutionChatGpt.Core.Interfaces
{
    public interface IShopCartManager
    {
        void Add(AddProductDTO product);

        List<ProductDetailsDTO> GetAll();

        decimal GetTotalCost();

        void DeleteAll();

        void Delete(long productId);

        bool Exists(long productId);
    }
}
