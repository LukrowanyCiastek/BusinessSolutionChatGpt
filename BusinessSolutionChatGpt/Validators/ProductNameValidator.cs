using Microsoft.Extensions.Localization;

namespace BusinessSolutionChatGpt.Validators
{
    internal class ProductNameValidator : NotNullOrEmptyStringValidator
    {
        public ProductNameValidator(IStringLocalizer localizer)
            : base(
                  localizer["ProductMissingNameValidationMessage"],
                  localizer["ProductEmptyNameValidationMessage"]
                  )
        {
        }
    }
}
