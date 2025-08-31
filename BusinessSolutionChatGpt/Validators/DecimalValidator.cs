using BusinessSolutionChatGpt.Validators.Interfaces;

namespace BusinessSolutionChatGpt.Validators
{
    internal class DecimalValidator : IValidator<string>
    {
        bool IValidator<string>.IsValid(string? input) => decimal.TryParse(input, out decimal result);
    }
}
