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

        public void Add(Product product) => products.Add(product);

        public List<Product> GetAll() => products.ToList();

        public decimal GetAllPrice() => products.Sum(x => x.Price);
    }
}
