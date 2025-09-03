using BusinessSolutionChatGpt.Core.DTO.Product;

namespace BusinessSolutionChatGpt.Core.Services.Interfaces
{
    public interface IAddProductService
    {
        void Add(AddProductDTO product);
    }
}
