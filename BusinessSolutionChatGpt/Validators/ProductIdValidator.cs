using BusinessSolutionChatGpt.Interfaces;
using BusinessSolutionChatGpt.Parsers.Interfaces;
using BusinessSolutionChatGpt.Validators.Interfaces;

namespace BusinessSolutionChatGpt.Validators
{
    internal class ProductIdValidator : IValidator<string>
    {
        private readonly IShopCartManager shopCartManager;
        private readonly IParser<int> integerParser;
        private readonly IValidator<string> integerValidator;

        public ProductIdValidator(IParser<int> integerParser, IShopCartManager shopCartManager) 
        {
            this.integerValidator = new IndexValidator(integerParser);
            this.shopCartManager = shopCartManager;
            this.integerParser = integerParser;
        }

        bool IValidator<string>.IsValid(string? input)
        {
            if (integerValidator.IsValid(input))
            {
                int value = this.integerParser.Parse(input!);
                return shopCartManager.Exists(value - 1);
            }

            return false;
        }
    }
}
