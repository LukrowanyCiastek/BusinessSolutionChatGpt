using BusinessSolutionChatGpt.Parsers.Interfaces;
using System.Globalization;

namespace BusinessSolutionChatGpt.Parsers
{
    internal class DecimalParser : IParser<decimal>
    {
        decimal IParser<decimal>.Parse(string input) => decimal.Parse(input, CultureInfo.InvariantCulture);
    }
}
