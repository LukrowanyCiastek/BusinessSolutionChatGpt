using BusinessSolutionChatGpt.Infrastructure.Interfacrs;
using BusinessSolutionChatGpt.Model;
using Newtonsoft.Json;

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
            var products = shopCartManager.GetAll();
            if (products.Count == 0)
            {
                output.WriteLine("koszyk jest pusty");
            }

            foreach(Product product in shopCartManager.GetAll())
            {
                output.WriteLine(JsonConvert.SerializeObject(product));
            }
        }
    }
}
