namespace BusinessSolutionChatGpt.Parsers.Interfaces
{
    internal interface IParser<T>
    {
        T Parse(string input);
    }
}
