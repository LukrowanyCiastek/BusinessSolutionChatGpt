using BusinessSolutionChatGpt.Core.DTO.Product;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace BusinessSolutionChatGpt.Core.Validators
{
    public class AddProductValidator : AbstractValidator<AddProductDTO>
    {
        public AddProductValidator(IStringLocalizer localizer)
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(localizer["ProductEmptyNameValidationMessage"])
                .NotNull().WithMessage(localizer["ProductMissingNameValidationMessage"]);

            RuleFor(x => x.Price)
                .Must((dto, value) => dto.Price > 0).WithMessage(localizer["ProductNotPositivePriceValidationMessage"]);
        }
    }
}
