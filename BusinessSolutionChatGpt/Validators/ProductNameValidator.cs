using Microsoft.Extensions.Localization;

namespace BusinessSolutionChatGpt.Validators
{
    internal class ProductNameValidator : NotNullOrEmptyStringValidator
    {
        public ProductNameValidator(IStringLocalizer<BusinessSolutionChatGpt.Resources.SharedResource> localizer)
            : base(
                  localizer["ProductMissingNameValidationMessage"],
                  localizer["ProductEmptyNameValidationMessage"]
                  )
        {
        }
    }
}
