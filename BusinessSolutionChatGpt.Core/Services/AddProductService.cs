using BusinessSolutionChatGpt.Core.DTO.Product;
using BusinessSolutionChatGpt.Core.Interfaces;
using BusinessSolutionChatGpt.Core.Model;
using BusinessSolutionChatGpt.Core.Services.Interfaces;

namespace BusinessSolutionChatGpt.Core.Services
{
    public class AddProductService : IAddProductService
    {
        private readonly IProductRepository repository;

        public AddProductService(IProductRepository repository)
        {
            this.repository = repository;
        }

        void IAddProductService.Add(AddProductDTO? product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product), "Product can't be null");
            }

            var model = new Product
            {
                Name = product.Name,
                Price = product.Price,
            };

            repository.Add(model);
        }
    }
}
