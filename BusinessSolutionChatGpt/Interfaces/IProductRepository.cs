using BusinessSolutionChatGpt.Model;

namespace BusinessSolutionChatGpt.Interfaces
{
    internal interface IProductRepository
    {
        void Add(Product product);

        List<Product> GetAll();

        decimal GetAllPrice();
    }
}
