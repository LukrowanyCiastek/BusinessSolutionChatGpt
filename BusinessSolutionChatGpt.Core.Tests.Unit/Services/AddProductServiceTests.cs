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
    public class AddProductServiceTests : BaseFixture
    {
        [Test]
        public void Add_GivenAddProductIsNull_ThrowsNullArgumentException()
        {
            Action act = () =>
            {
                IAddProductService addProductService = Fixture.Create<AddProductService>();
                addProductService.Add(null);
            };

            act.Should()
                .Throw<ArgumentNullException>()
                .WithMessage("Product can't be null (Parameter 'product')");
        }

        [Test]
        public void Add_GivenAddProductIsNotNull_ProductIsAddedToRepository()
        {
            var addProductDto = Fixture.Create<AddProductDTO>();
            var mockProductRepository = Fixture.FreezeMock<IProductRepository>();
            IAddProductService addProductService = Fixture.Create<AddProductService>();

            addProductService.Add(addProductDto);

            mockProductRepository.Received().Add(Arg.Is<Product>(x => x.Name == addProductDto.Name && x.Price == addProductDto.Price));
        }
    }
}
