using BusinessSolutionChatGpt.DTO.Product;

namespace BusinessSolutionChatGpt.Interfaces
{
    internal interface IShopCartManager
    {
        void Add(AddProductDTO product);

        List<ProductDetailsDTO> GetAll();

        decimal GetTotalCost();

        void DeleteAll();

        void Delete(int productId);

        bool Exists(int productId);
    }
}
