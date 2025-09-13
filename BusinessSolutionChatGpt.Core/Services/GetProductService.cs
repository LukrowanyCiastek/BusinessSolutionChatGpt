using BusinessSolutionChatGpt.Core.DTO.Product;
using BusinessSolutionChatGpt.Core.Interfaces;
using BusinessSolutionChatGpt.Core.Services.Interfaces;

namespace BusinessSolutionChatGpt.Core.Services
{
    public class GetProductService : IGetProductService
    {
        private readonly IProductRepository repository;

        public GetProductService(IProductRepository repository) 
        {
            this.repository = repository;
        }

        bool IGetProductService.Exists(long id) => repository.Exists(id);

        List<ProductDetailsDTO> IGetProductService.GetAll() => repository.GetAll().Select(x => new ProductDetailsDTO { Name = x.Name!, Price = x.Price }).ToList();

        decimal IGetProductService.GetPriceAll() => repository.GetAllPrice();
    }
}
