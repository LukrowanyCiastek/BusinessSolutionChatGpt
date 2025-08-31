using BusinessSolutionChatGpt.DTO.Product;
using BusinessSolutionChatGpt.Interfaces;
using BusinessSolutionChatGpt.Model;
using BusinessSolutionChatGpt.Services.Interfaces;

namespace BusinessSolutionChatGpt.Services
{
    internal class AddProductService : IAddProductService
    {
        private readonly IProductRepository repository;

        public AddProductService(IProductRepository repository)
        {
            this.repository = repository;
        }

        void IAddProductService.Add(AddProductDTO product)
        {
            var model = new Product
            {
                Name = product.Name,
                Price = product.Price,
            };

            this.repository.Add(model);
        }
    }
}
