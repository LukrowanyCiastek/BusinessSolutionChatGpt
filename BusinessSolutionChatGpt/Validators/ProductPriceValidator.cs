using Microsoft.Extensions.Localization;

namespace BusinessSolutionChatGpt.Validators
{
    internal class ProductPriceValidator : PositiveDecimalValidator
    {
        public ProductPriceValidator(IStringLocalizer<BusinessSolutionChatGpt.Resources.SharedResource> localizer)
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
