namespace BusinessSolutionChatGpt
{
    internal interface IInputRetriever<T>
    {
        T TryGet();
    }
}
