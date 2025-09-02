using BusinessSolutionChatGpt.DTO.Input;
using BusinessSolutionChatGpt.Interfaces;
using FluentValidation;
using System.Globalization;

namespace BusinessSolutionChatGpt.Validators
{
    internal class ProductIdValidator : AbstractValidator<InputDTO<int>>
    {
        public ProductIdValidator(string nullMessage, string emptyMessage, string wrongIdentifier, string entyDoesNotExistMessage, IShopCartManager shopCartManager)
        {
            RuleFor(x => x.Raw)
                .NotNull().WithMessage(nullMessage)
                .NotEmpty().WithMessage(emptyMessage)
                .Must((dto, value) =>
                {
                    if (int.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out var result))
                    {
                        dto.Value = result;
                        return dto.Value > 0;
                    }
                    return false;
                })
                .WithMessage(wrongIdentifier)
                .Must((dto, value) => shopCartManager.Exists(dto.Value - 1))
                .WithMessage(entyDoesNotExistMessage);
        }
    }
}
