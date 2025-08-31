using BusinessSolutionChatGpt.Validators.Interfaces;
using System.Globalization;

namespace BusinessSolutionChatGpt.Validators
{
    internal class IntegerValidator : IValidator<string>
    {
        bool IValidator<string>.IsValid(string? input) => int.TryParse(input, CultureInfo.InvariantCulture, out int result);
    }
}
