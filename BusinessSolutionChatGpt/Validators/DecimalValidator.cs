using BusinessSolutionChatGpt.Validators.Interfaces;
using System.Globalization;

namespace BusinessSolutionChatGpt.Validators
{
    internal class DecimalValidator : IValidator<string>
    {
        private readonly IValidator<string> validator;

        public DecimalValidator()
        {
            validator = new NotNullOrEmptyStringValidator();
        }
        bool IValidator<string>.IsValid(string? input)
        {
            if (validator.IsValid(input))
            {
                return decimal.TryParse(input, CultureInfo.InvariantCulture, out decimal result);
            }

            return false;
        }
    }
}
