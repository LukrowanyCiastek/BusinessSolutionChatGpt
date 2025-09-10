using BusinessSolutionChatGpt.Core.Interfaces;
using BusinessSolutionChatGpt.Core.Model;

namespace BusinessSolutionChatGpt.Core
{
    public class ProductRepository : IProductRepository
    {
        private readonly IList<Product> products;

        public ProductRepository(IList<Product> products)
        {
            this.products = products;
        }

        void IProductRepository.Add(Product product) => products.Add(product);

        void IProductRepository.Delete(long id) => products.RemoveAt((int)id);

        void IProductRepository.DeleteAll() => products.Clear();

        bool IProductRepository.Exists(long id) => id >= 0 && id < products.Count;

        List<Product> IProductRepository.GetAll() => products.ToList();

        decimal IProductRepository.GetAllPrice() => products.Sum(x => x.Price);
    }
}
