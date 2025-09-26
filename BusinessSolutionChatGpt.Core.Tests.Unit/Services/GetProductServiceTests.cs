using AutoFixture;
using BusinessSolutionChatGpt.Core.DTO.Product;
using BusinessSolutionChatGpt.Core.Interfaces;
using BusinessSolutionChatGpt.Core.Model;
using BusinessSolutionChatGpt.Core.Services;
using BusinessSolutionChatGpt.Core.Services.Interfaces;
using BusinessSolutionChatGpt.Tests.Core;
using FluentAssertions;
using NSubstitute;

namespace BusinessSolutionChatGpt.Core.Tests.Unit.Services
{
    [TestFixture]
    public class GetProductServiceTests : BaseFixture
    {
        [Test]
        public void Exists_GivenId_RepositoryWasUsedWithSameId()
        {
            var id = Fixture.Create<int>();
            var mockProductRepository = Fixture.FreezeMock<IProductRepository>();
            IGetProductService service = Fixture.Create<GetProductService>();

            service.Exists(id);

            mockProductRepository.Received().Exists(id);
        }

        [Test]
        public void GetAll_Called_RepositoryWasUsedReturnsProductDetails()
        {
            var products = Fixture.CreateMany<Product>().ToList();
            var mockProductRepository = Fixture.FreezeMock<IProductRepository>();
            mockProductRepository.GetAll().Returns(products);
            IGetProductService service = Fixture.Create<GetProductService>();

            var actual = service.GetAll();

            mockProductRepository.Received().GetAll();
            products.Select(x => new ProductDetailsDTO { Name = x.Name, Price = x.Price }).ToList().Should().BeEquivalentTo(actual);
        }

        [Test]
        public void GetTotalCost_Called_RepositoryWasUsedReturnsTotalCost()
        {
            var expected = Fixture.Create<decimal>();
            var mockProductRepository = Fixture.FreezeMock<IProductRepository>();
            mockProductRepository.GetTotalPrice().Returns(expected);
            IGetProductService service = Fixture.Create<GetProductService>();

            var actual = service.GetTotalCost();

            mockProductRepository.Received().GetTotalPrice();
            expected.Should().Be(actual);
        }
    }
}
