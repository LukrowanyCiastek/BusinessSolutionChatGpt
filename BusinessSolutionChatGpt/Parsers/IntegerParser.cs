using BusinessSolutionChatGpt.Parsers.Interfaces;
using BusinessSolutionChatGpt.Validators;
using BusinessSolutionChatGpt.Validators.Interfaces;
using System.Globalization;

namespace BusinessSolutionChatGpt.Parsers
{
    internal class IntegerParser : IParser<int>
    {
        private readonly IValidator<string> validator;

        public IntegerParser()
        {
            validator = new IntegerValidator();
        }

        int IParser<int>.Parse(string input)
        {
            if (validator.IsValid(input))
            {
                return int.Parse(input, CultureInfo.InvariantCulture);
            }

            throw new ArgumentException("Argument nie jest liczbą integer", nameof(input));
        }
    }
}
