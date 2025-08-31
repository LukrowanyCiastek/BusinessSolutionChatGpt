using BusinessSolutionChatGpt.Parsers.Interfaces;
using BusinessSolutionChatGpt.Validators;
using BusinessSolutionChatGpt.Validators.Interfaces;
using System.Globalization;

namespace BusinessSolutionChatGpt.Parsers
{
    internal class DecimalParser : IParser<decimal>
    {
        private readonly IValidator<string> validator;

        public DecimalParser()
        {
            this.validator = new DecimalValidator();
        }

        decimal IParser<decimal>.Parse(string input)
        {
            if (validator.IsValid(input))
            {
                return decimal.Parse(input, CultureInfo.InvariantCulture);
            }

            throw new ArgumentException("Argument nie jest liczbą decimal", nameof(input));
        }
    }
}
