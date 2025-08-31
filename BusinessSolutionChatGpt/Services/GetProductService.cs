using BusinessSolutionChatGpt.DTO.Product;
using BusinessSolutionChatGpt.Interfaces;
using BusinessSolutionChatGpt.Services.Interfaces;

namespace BusinessSolutionChatGpt.Services
{
    internal class GetProductService : IGetProductService
    {
        private readonly IProductRepository repository;

        public GetProductService(IProductRepository repository) 
        {
            this.repository = repository;
        }

        public List<ProductDetailsDTO> GetAll() => repository.GetAll().Select(x => new ProductDetailsDTO { Name = x.Name!, Price = x.Price }).ToList();

        public decimal GetPriceAll() => repository.GetAllPrice();
    }
}
