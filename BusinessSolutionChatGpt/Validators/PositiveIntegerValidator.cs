using BusinessSolutionChatGpt.Parsers.Interfaces;
using BusinessSolutionChatGpt.Validators.Interfaces;

namespace BusinessSolutionChatGpt.Validators
{
    internal class PositiveIntegerValidator : IValidator<string>
    {
        private readonly IValidator<string> integerValidator;
        private readonly IParser<int> parser;

        public PositiveIntegerValidator(IParser<int> parser)
        {
            integerValidator = new IntegerValidator();
            this.parser = parser;
        }

        bool IValidator<string>.IsValid(string? input)
        {
            if (integerValidator.IsValid(input))
            {
                int value = this.parser.Parse(input!);
                return value > 0;
            }

            return false;
        }
    }
}
