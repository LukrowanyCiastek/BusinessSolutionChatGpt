using Microsoft.Extensions.Localization;

namespace BusinessSolutionChatGpt.Validators
{
    internal class ProductPricelValidator : PositiveDecimalValidator
    {
        public ProductPricelValidator(IStringLocalizer<Resources.Resources> localizer)
            : base(
                  localizer["ProductMissingPriceValidationMessage"],
                  localizer["ProductEmptyPriceValidationMessage"],
                  localizer["ProductNotDecimalPriceValidationMessage"],
                  localizer["ProductNotPositivePriceValidationMessage"]
              )
        {
        }
    }
}
