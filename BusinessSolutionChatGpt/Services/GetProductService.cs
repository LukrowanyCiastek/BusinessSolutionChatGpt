using BusinessSolutionChatGpt.DTO.Product;
using BusinessSolutionChatGpt.Model;
using BusinessSolutionChatGpt.Services.Interfaces;

namespace BusinessSolutionChatGpt.Services
{
    internal class GetProductService : IGetProductService
    {
        private readonly IList<Product> products;

        public GetProductService(IList<Product> products) 
        {
            this.products = products;
        }

        public List<ProductDetailsDTO> GetAll() => products.Select(x => new ProductDetailsDTO { Name = x.Name!, Price = x.Price }).ToList();

        public decimal GetPriceAll() => products.Sum(x => x.Price);
    }
}
