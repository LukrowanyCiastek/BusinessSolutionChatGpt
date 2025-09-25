using BusinessSolutionChatGpt.Core.Interfaces;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace BusinessSolutionChatGpt.Core.Validators
{
    public class ProductExistValidator : AbstractValidator<long>
    {
        public ProductExistValidator(IStringLocalizer localizer, IShopCartManager shopCartManager) 
        {
            RuleFor(x => x)
                .Must((x) => x > 0).WithMessage("Identyfiaktor musi być większy od 0")
                .Must(shopCartManager.Exists).WithMessage(localizer.GetString("ProductNotExistValidationMessage").Value);
        }
    }
}
