using AutoFixture;
using BusinessSolutionChatGpt.Console.Infrastructure.Interfaces;
using BusinessSolutionChatGpt.Core.DTO.Product;
using BusinessSolutionChatGpt.Core.Interfaces;
using BusinessSolutionChatGpt.Tests.Core;
using NSubstitute;
using System.Globalization;

namespace BusinessSolutionChatGpt.Console.Tests.Unit
{
    [TestFixture]
    public class ShopCartPrinterTests : BaseFixture
    {
        [Test]
        public void Print_CartIsEmpty_PrintShopCartIsEmpty()
        {
            var mockOutput = Fixture.FreezeMock<IOutput>();
            var mockShopCartManager = Fixture.FreezeMock<IShopCartManager>();
            mockShopCartManager.GetAll().Returns(new List<ProductDetailsDTO>());
            var printer = Fixture.Create<ShopCartPrinter>();

            printer.Print();

            mockOutput.Received().WriteLine(string.Empty);
            mockOutput.Received().WriteLine("koszyk jest pusty");
        }

        [Test]
        public void Print_CartIsNotEmpty_PrintShopCartIsEmpty()
        {
            var mockOutput = Fixture.FreezeMock<IOutput>();
            var mockShopCartManager = Fixture.FreezeMock<IShopCartManager>();
            var products = Fixture.CreateMany<ProductDetailsDTO>().ToList();
            mockShopCartManager.GetAll().Returns(products);
            var printer = Fixture.Create<ShopCartPrinter>();

            printer.Print();

            mockOutput.Received().WriteLine(string.Empty);
            foreach (var entry in products.Select((product, index) => new { product, index }))
            {
                mockOutput.Received().WriteLine($"Produkt {entry.index + 1}");
                mockOutput.Received().WriteLine($"Nazwa {entry.product.Name}");
                mockOutput.Received().WriteLine($"Cena {entry.product.Price.ToString(CultureInfo.InvariantCulture)}");
            }
        }
    }
}
