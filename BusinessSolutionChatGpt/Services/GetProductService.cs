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

        bool IGetProductService.Exists(int id) => repository.Exists(id);

        List<ProductDetailsDTO> IGetProductService.GetAll() => repository.GetAll().Select(x => new ProductDetailsDTO { Name = x.Name!, Price = x.Price }).ToList();

        decimal IGetProductService.GetPriceAll() => repository.GetAllPrice();
    }
}
