using BusinessSolutionChatGpt.DTO.Product;

namespace BusinessSolutionChatGpt.Services.Interfaces
{
    internal interface IGetProductService
    {
        List<ProductDetailsDTO> GetAll();

        decimal GetPriceAll();

        bool Exists(int id);
    }
}
