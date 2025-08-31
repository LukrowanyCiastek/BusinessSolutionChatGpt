using BusinessSolutionChatGpt.Validators.Interfaces;

namespace BusinessSolutionChatGpt.Validators
{
    internal class NotNullOrEmptyStringValidator : IValidator<string>
    {
        bool IValidator<string>.IsValid(string? input) => !string.IsNullOrEmpty(input);
    }
}
