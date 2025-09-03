using BusinessSolutionChatGpt.DTO.Input;
using BusinessSolutionChatGpt.Interfaces;
using FluentValidation;
using Microsoft.Extensions.Localization;
using System.Globalization;

namespace BusinessSolutionChatGpt.Validators
{
    internal class ProductIdValidator : AbstractValidator<InputDTO<int>>
    {
        public ProductIdValidator(IStringLocalizer localizer, IShopCartManager shopCartManager)
        {
            RuleFor(x => x.Raw)
                .NotNull().WithMessage(localizer["ProductMissingIdentifierValidationMessage"])
                .NotEmpty().WithMessage(localizer["ProductEmptyIdentifierValidationMessage"])
                .Must((dto, value) =>
                {
                    if (int.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out var result))
                    {
                        dto.Value = result;
                        return dto.Value > 0;
                    }
                    return false;
                })
                .WithMessage(localizer["ProductNotIntegerIdentifierValidationMessage"])
                .Must((dto, value) => shopCartManager.Exists(dto.Value - 1))
                .WithMessage(localizer["ProductNotExistValidationMessage"]);
        }
    }
}
