using BusinessSolutionChatGpt.Model;

namespace BusinessSolutionChatGpt.Interfaces
{
    internal interface IProductRepository
    {
        void Add(Product product);

        List<Product> GetAll();

        decimal GetAllPrice();

        void DeleteAll();

        void Delete(int id);

        bool Exists(int id);
    }
}
