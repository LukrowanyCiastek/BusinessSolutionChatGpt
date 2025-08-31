using BusinessSolutionChatGpt.DTO.Product;
using BusinessSolutionChatGpt.Model;
using BusinessSolutionChatGpt.Services.Interfaces;

namespace BusinessSolutionChatGpt.Services
{
    internal class AddProductService : IAddProductService
    {
        private readonly IList<Product> products;

        public AddProductService(IList<Product> products)
        {
            this.products = products;
        }

        void IAddProductService.Add(AddProductDTO product)
        {
            var model = new Product
            {
                Name = product.Name,
                Price = product.Price,
            };

            this.products.Add(model);
        }
    }
}
