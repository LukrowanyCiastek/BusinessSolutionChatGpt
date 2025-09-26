using AutoFixture;
using BusinessSolutionChatGpt.Core.DTO.Product;
using BusinessSolutionChatGpt.Core.Interfaces;
using BusinessSolutionChatGpt.Core.Services.Interfaces;
using BusinessSolutionChatGpt.Tests.Core;
using FluentAssertions;
using NSubstitute;

namespace BusinessSolutionChatGpt.Core.Tests.Unit
{
    [TestFixture]
    public class ShopCartManagerTests : BaseFixture
    {
        [Test]
        public void Add_GiveProduct_ServiceUsedSameProduct()
        {
            var product = Fixture.Create<AddProductDTO>();
            var addProductService = Fixture.FreezeMock<IAddProductService>();
            IShopCartManager manager = Fixture.Create<ShopCartManager>();

            manager.Add(product);

            addProductService.Received().Add(product);
        }

        [Test]
        public void GetAll_Called_ReturnSameProductsLikeService()
        {
            var expected = Fixture.CreateMany<ProductDetailsDTO>().ToList();
            var getProductService = Fixture.FreezeMock<IGetProductService>();
            getProductService.GetAll().Returns(expected);
            IShopCartManager manager = Fixture.Create<ShopCartManager>();

            var actual = manager.GetAll();

            expected.Should().BeEquivalentTo(actual);
        }

        [Test]
        public void GetTotalCost_Called_ReturnSameTotalCostLikeService()
        {
            var expected = Fixture.Create<decimal>();
            var getProductService = Fixture.FreezeMock<IGetProductService>();
            getProductService.GetTotalCost().Returns(expected);
            IShopCartManager manager = Fixture.Create<ShopCartManager>();

            var actual = manager.GetTotalCost();

            expected.Should().Be(actual);
        }

        [Test]
        public void Exist_GivenId_ServiceWasUsedWithSameId()
        {
            var productId = Fixture.Create<long>();
            var getProductService = Fixture.FreezeMock<IGetProductService>();
            IShopCartManager manager = Fixture.Create<ShopCartManager>();

            manager.Exists(productId);

            getProductService.Received().Exists(productId);
        }

        [Test]
        public void DeleteAll_Called_ServiceWasUsed()
        {
            var deleteProductService = Fixture.FreezeMock<IDeleteProductService>();
            IShopCartManager manager = Fixture.Create<ShopCartManager>();

            manager.DeleteAll();

            deleteProductService.Received().DeleteAll();
        }

        [Test]
        public void Delete_GivenId_ServiceWasUsedWithSameId()
        {
            var productId = Fixture.Create<long>();
            var deleteProductService = Fixture.FreezeMock<IDeleteProductService>();
            IShopCartManager manager = Fixture.Create<ShopCartManager>();

            manager.Delete(productId);

            deleteProductService.Received().Delete(productId);
        }
    }
}
