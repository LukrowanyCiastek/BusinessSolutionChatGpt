using Microsoft.Extensions.Localization;

namespace BusinessSolutionChatGpt.Validators
{
    internal class ProductPriceValidator : PositiveDecimalValidator
    {
        public ProductPriceValidator(IStringLocalizer localizer)
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
