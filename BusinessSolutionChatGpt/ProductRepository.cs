using BusinessSolutionChatGpt.Interfaces;
using BusinessSolutionChatGpt.Model;

namespace BusinessSolutionChatGpt
{
    internal class ProductRepository : IProductRepository
    {
        private readonly IList<Product> products;

        public ProductRepository(IList<Product> products)
        {
            this.products = products;
        }

        void IProductRepository.Add(Product product) => products.Add(product);

        void IProductRepository.Delete(int id) => products.RemoveAt(id - 1);

        void IProductRepository.DeleteAll() => products.Clear();

        bool IProductRepository.Exists(int id) => id >= 0 && id < products.Count;

        List<Product> IProductRepository.GetAll() => products.ToList();

        decimal IProductRepository.GetAllPrice() => products.Sum(x => x.Price);
    }
}
