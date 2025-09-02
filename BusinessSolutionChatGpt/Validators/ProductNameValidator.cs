using Microsoft.Extensions.Localization;

namespace BusinessSolutionChatGpt.Validators
{
    internal class ProductNameValidator : NotNullOrEmptyStringValidator
    {
        public ProductNameValidator(IStringLocalizer<Resources.Resources> localizer)
            : base(
                  localizer["ProductMissingNameValidationMessage"],
                  localizer["ProductEmptyNameValidationMessage"]
                  )
        {
        }
    }
}
