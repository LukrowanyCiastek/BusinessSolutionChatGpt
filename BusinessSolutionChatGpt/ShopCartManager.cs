using BusinessSolutionChatGpt.Model;

namespace BusinessSolutionChatGpt
{
    internal class ShopCartManager : IShopCartManager
    {
        private readonly List<Product> products;

        public ShopCartManager()
        {
            products = new List<Product>();
        }

        void IShopCartManager.Add(Product product)
        {
            products.Add(product);
        }

        List<Product> IShopCartManager.GetAll()
        {
            return products;
        }
    }
}
