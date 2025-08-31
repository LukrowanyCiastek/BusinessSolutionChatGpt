namespace BusinessSolutionChatGpt
{
    internal class ShopCartCalculator
    {
        private readonly IShopCartManager shopCartManager;

        public ShopCartCalculator(IShopCartManager shopCartManager)
        {
            this.shopCartManager = shopCartManager;
        }

        internal decimal GetTotalCost() => shopCartManager.GetAll().Sum(x => x.Price);
    }
}
