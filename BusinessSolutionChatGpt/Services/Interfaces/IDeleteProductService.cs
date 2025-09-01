namespace BusinessSolutionChatGpt.Services.Interfaces
{
    internal interface IDeleteProductService
    {
        void DeleteAll();

        void Delete(int productId);
    }
}
