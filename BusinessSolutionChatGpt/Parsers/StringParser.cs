using BusinessSolutionChatGpt.Parsers.Interfaces;

namespace BusinessSolutionChatGpt.Parsers
{
    internal class StringParser : IParser<string>
    {
        string IParser<string>.Parse(string input) => input;
    }
}
