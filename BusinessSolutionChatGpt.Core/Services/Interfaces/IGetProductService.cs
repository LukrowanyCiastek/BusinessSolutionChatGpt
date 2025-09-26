using BusinessSolutionChatGpt.Core.DTO.Product;

namespace BusinessSolutionChatGpt.Core.Services.Interfaces
{
    public interface IGetProductService
    {
        List<ProductDetailsDTO> GetAll();

        decimal GetTotalCost();

        bool Exists(long id);
    }
}
