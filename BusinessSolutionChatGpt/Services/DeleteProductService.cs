using BusinessSolutionChatGpt.Interfaces;
using BusinessSolutionChatGpt.Services.Interfaces;

namespace BusinessSolutionChatGpt.Services
{
    internal class DeleteProductService : IDeleteProductService
    {
        private readonly IProductRepository productRepository;

        public DeleteProductService(IProductRepository productRepository) 
        {
            this.productRepository = productRepository;
        }

        void IDeleteProductService.Delete(int productId) => productRepository.Delete(productId);

        void IDeleteProductService.DeleteAll() => productRepository.DeleteAll();
    }
}
