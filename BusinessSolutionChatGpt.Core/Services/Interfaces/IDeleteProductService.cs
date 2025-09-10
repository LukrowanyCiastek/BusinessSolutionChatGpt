namespace BusinessSolutionChatGpt.Core.Services.Interfaces
{
    public interface IDeleteProductService
    {
        void DeleteAll();

        void Delete(long productId);
    }
}
