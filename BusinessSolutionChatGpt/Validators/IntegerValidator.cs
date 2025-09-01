using BusinessSolutionChatGpt.Validators.Interfaces;
using System.Globalization;

namespace BusinessSolutionChatGpt.Validators
{
    internal class IntegerValidator : IValidator<string>
    {
        private readonly IValidator<string> validator;

        public IntegerValidator()
        {
            validator = new NotNullOrEmptyStringValidator();
        }
        bool IValidator<string>.IsValid(string? input)
        {
            if (validator.IsValid(input))
            {
                return int.TryParse(input, CultureInfo.InvariantCulture, out int result);
            }

            return false;
        }
    }
}
