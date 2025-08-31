using BusinessSolutionChatGpt.Model;

namespace BusinessSolutionChatGpt
{
    internal interface IShopCartManager
    {
        void Add(Product product);

        List<Product> GetAll();
    }
}
