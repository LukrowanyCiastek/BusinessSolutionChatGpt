using BusinessSolutionChatGpt.Infrastructure.Interfaces;
using BusinessSolutionChatGpt.Validators;
using Microsoft.Extensions.Localization;

namespace BusinessSolutionChatGpt
{
    internal class ProductPriceLoopDataRetriever : LoopDataRetriever<decimal>
    {
        public ProductPriceLoopDataRetriever(IOutput output,
            IInput input,
            ProductPricelValidator dataValidator,
            IStringLocalizer<Resources.Resources> localizer)
            : base(output, input, dataValidator, localizer["ProductPriceInstruction"])
        {
        }
    }
}
