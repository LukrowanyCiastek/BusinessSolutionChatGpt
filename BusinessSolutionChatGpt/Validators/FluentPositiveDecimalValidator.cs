using BusinessSolutionChatGpt.DTO.Input;
using FluentValidation;
using System.Globalization;

namespace BusinessSolutionChatGpt.Validators
{
    internal class FluentPositiveDecimalValidator : AbstractValidator<InputDTO<decimal>>
    {
        public FluentPositiveDecimalValidator(string nullMessage, string emptyMessage, string parseMessage, string notPositiveMessage)
        {
            RuleFor(x => x.Raw)
                .NotNull().WithMessage(nullMessage)
                .NotEmpty().WithMessage(emptyMessage)
                .Must((dto, value) =>
                {
                    if (decimal.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out var result))
                    {
                        dto.Value = result;
                        return true;
                    }
                    return false;
                })
                .WithMessage(parseMessage)
                .Must((dto, value) => dto.Value > 0)
                .WithMessage(notPositiveMessage);
        }
    }
}
