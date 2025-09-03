using BusinessSolutionChatGpt.DTO.Input;
using FluentValidation;

namespace BusinessSolutionChatGpt.Validators
{
    internal class NotNullOrEmptyStringValidator : AbstractValidator<InputDTO<string>>
    {
        public NotNullOrEmptyStringValidator(string notNullMessage, string emptyMessage)
        {
            RuleFor(x => x.Raw)
                .NotNull().WithMessage(notNullMessage)
                .NotEmpty().WithMessage(emptyMessage)
                .Must((dto, value) =>
                {
                    dto.Value = value;
                    return true;
                });
        }
    }
}
