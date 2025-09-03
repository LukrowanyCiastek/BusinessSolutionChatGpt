using BusinessSolutionChatGpt.Core.Interfaces;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace BusinessSolutionChatGpt.Core.Validators
{
    public class ProductExistValidator : AbstractValidator<long>
    {
        public ProductExistValidator(IShopCartManager shopCartManager, IStringLocalizer localizer) 
        {
            RuleFor(x => x)
                .Must((x, value) => x > 0).WithMessage("Identyfiaktor musi być większy od 0")
                .Must((x, value) => shopCartManager.Exists(x)).WithMessage(localizer["ProductNotExistValidationMessage"]);
        }
    }
}
