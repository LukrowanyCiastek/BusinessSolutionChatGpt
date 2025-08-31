using BusinessSolutionChatGpt.Infrastructure.Interfaces;
using BusinessSolutionChatGpt.Interfaces;

namespace BusinessSolutionChatGpt
{
    internal class ShopCartPrinter
    {
        private readonly IOutput output;
        private readonly IShopCartManager shopCartManager;

        public ShopCartPrinter(IOutput output, IShopCartManager shopCartManager)
        {
            this.output = output;
            this.shopCartManager = shopCartManager;
        }

        internal void Print()
        {
            output.WriteLine(string.Empty);
            var products = shopCartManager.GetAll();
            if (products.Count == 0)
            {
                output.WriteLine("koszyk jest pusty");
            }

            foreach(var entry in shopCartManager.GetAll().Select((product, index) => new { product, index}))
            {
                output.WriteLine($"Produkt {entry.index}");
                output.WriteLine($"Nazwa {entry.product.Name}");
                output.WriteLine($"Cena {entry.product.Price}");
            }
        }
    }
}
