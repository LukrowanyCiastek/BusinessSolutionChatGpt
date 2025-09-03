using BusinessSolutionChatGpt.Core.Model;

namespace BusinessSolutionChatGpt.Core.Interfaces
{
    public interface IProductRepository
    {
        void Add(Product product);

        List<Product> GetAll();

        decimal GetAllPrice();

        void DeleteAll();

        void Delete(long id);

        bool Exists(long id);
    }
}
