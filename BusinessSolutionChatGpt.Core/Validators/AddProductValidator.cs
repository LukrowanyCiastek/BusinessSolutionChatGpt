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
                .NotNull().WithMessage(localizer.GetString("ProductMissingNameValidationMessage").Value)
                .NotEmpty().WithMessage(localizer.GetString("ProductEmptyNameValidationMessage").Value);

            RuleFor(x => x.Price)
                .Must((dto, value) => dto.Price > 0).WithMessage(localizer.GetString("ProductNotPositivePriceValidationMessage").Value);
        }
    }
}
