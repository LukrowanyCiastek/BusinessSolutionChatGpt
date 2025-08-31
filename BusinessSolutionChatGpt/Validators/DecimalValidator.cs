using BusinessSolutionChatGpt.Validators.Interfaces;
using System.Globalization;

namespace BusinessSolutionChatGpt.Validators
{
    internal class DecimalValidator : IValidator<string>
    {
        bool IValidator<string>.IsValid(string? input) => decimal.TryParse(input, CultureInfo.InvariantCulture, out decimal result);
    }
}
