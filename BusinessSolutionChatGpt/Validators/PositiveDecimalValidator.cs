using BusinessSolutionChatGpt.Parsers.Interfaces;
using BusinessSolutionChatGpt.Validators.Interfaces;

namespace BusinessSolutionChatGpt.Validators
{
    internal class PositiveDecimalValidator : IValidator<string>
    {
        private readonly IValidator<string> decimalValidator;
        private readonly IParser<decimal> parser;

        public PositiveDecimalValidator(IParser<decimal> parser)
        {
            this.decimalValidator = new DecimalValidator();
            this.parser = parser;
        }

        bool IValidator<string>.IsValid(string? input)
        {
            if(decimalValidator.IsValid(input))
            {
                decimal value = this.parser.Parse(input!);
                return value > 0;
            }

            return false;
        }
    }
}
