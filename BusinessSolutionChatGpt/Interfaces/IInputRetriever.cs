namespace BusinessSolutionChatGpt.Interfaces
{
    internal interface IInputRetriever<T>
    {
        T TryGet();
    }
}
