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

        void IProductRepository.Add(Product? product)
        {
            if(product == null)
            {
                throw new ArgumentNullException(nameof(product), "Product can't be null");
            }

            products.Add(product);
        }

        void IProductRepository.Delete(long id)
        {
            if (((IProductRepository)this).Exists(id))
            {
                products.RemoveAt((int)id);
            }
        }

        void IProductRepository.DeleteAll()
        {
            if(products.Count == 0)
            {
                return;
            }

            products.Clear();
        }

        bool IProductRepository.Exists(long id) => id >= 0 && id <= products.Count;

        List<Product> IProductRepository.GetAll() => [.. products];

        decimal IProductRepository.GetTotalPrice() => products.Sum(x => x.Price);
    }
}
