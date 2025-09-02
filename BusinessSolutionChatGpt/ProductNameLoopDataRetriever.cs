using BusinessSolutionChatGpt.Infrastructure.Interfaces;
using BusinessSolutionChatGpt.Validators;
using Microsoft.Extensions.Localization;

namespace BusinessSolutionChatGpt
{
    internal class ProductNameLoopDataRetriever : LoopDataRetriever<string>
    {
        public ProductNameLoopDataRetriever(IOutput output,
            IInput input,
            ProductNameValidator dataValidator,
            IStringLocalizer<Resources.SharedResource> localizer)
            : base(output, input, dataValidator, localizer["ProductNameInstruction"])
        {
        }
    }
}
