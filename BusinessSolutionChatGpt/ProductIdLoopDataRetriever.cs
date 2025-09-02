using BusinessSolutionChatGpt.Infrastructure.Interfaces;
using BusinessSolutionChatGpt.Validators;
using Microsoft.Extensions.Localization;

namespace BusinessSolutionChatGpt
{
    internal class ProductIdLoopDataRetriever : LoopDataRetriever<int>
    {
        public ProductIdLoopDataRetriever(IOutput output,
            IInput input,
            ProductIdValidator dataValidator,
            IStringLocalizer<Resources.Resources> localizer)
            : base(output, input, dataValidator, localizer["ProductIdentifierInstruction"])
        {
        }
    }
}
