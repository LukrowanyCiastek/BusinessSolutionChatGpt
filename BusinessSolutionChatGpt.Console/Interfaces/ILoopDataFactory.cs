namespace BusinessSolutionChatGpt.Console.Interfaces
{
    internal interface ILoopDataFactory
    {
        ILoopDataRetriever<T> Create<T>() where T : new();
    }
}
