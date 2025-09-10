using BusinessSolutionChatGpt.Core.Interfaces;
using BusinessSolutionChatGpt.Core.Services.Interfaces;

namespace BusinessSolutionChatGpt.Services
{
    public class DeleteProductService : IDeleteProductService
    {
        private readonly IProductRepository productRepository;

        public DeleteProductService(IProductRepository productRepository) 
        {
            this.productRepository = productRepository;
        }

        void IDeleteProductService.Delete(long productId) => productRepository.Delete(productId);

        void IDeleteProductService.DeleteAll() => productRepository.DeleteAll();
    }
}
