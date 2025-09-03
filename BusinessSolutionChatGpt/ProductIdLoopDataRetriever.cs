using BusinessSolutionChatGpt.Infrastructure.Interfaces;
using BusinessSolutionChatGpt.Validators;
using Microsoft.Extensions.Localization;

namespace BusinessSolutionChatGpt
{
    internal class ProductIdLoopDataRetriever : LoopDataRetriever<long>
    {
        public ProductIdLoopDataRetriever(IOutput output,
            IInput input,
            ProductIdValidator dataValidator,
            IStringLocalizer localizer)
            : base(output, input, dataValidator, localizer["ProductIdentifierInstruction"])
        {
        }
    }
}
