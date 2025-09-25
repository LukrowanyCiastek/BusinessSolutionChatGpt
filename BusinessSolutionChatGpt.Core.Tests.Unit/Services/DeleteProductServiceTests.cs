using AutoFixture;
using BusinessSolutionChatGpt.Core.Interfaces;
using BusinessSolutionChatGpt.Core.Services.Interfaces;
using BusinessSolutionChatGpt.Services;
using BusinessSolutionChatGpt.Tests.Core;
using NSubstitute;

namespace BusinessSolutionChatGpt.Core.Tests.Unit.Services
{
    [TestFixture]
    public class DeleteProductServiceTests : BaseFixture
    {
        [Test]
        public void Delete_GivenId_RepositoryWasUsedWithSameId()
        {
            var productId = Fixture.Create<long>();
            var productRepositoryMock = Fixture.FreezeMock<IProductRepository>();
            IDeleteProductService service = Fixture.Create<DeleteProductService>();

            service.Delete(productId);

            productRepositoryMock.Received().Delete(productId);
        }

        [Test]
        public void DeleteAll_GivenId_RepositoryWasUsed()
        {
            var productId = Fixture.Create<long>();
            var productRepositoryMock = Fixture.FreezeMock<IProductRepository>();
            IDeleteProductService service = Fixture.Create<DeleteProductService>();

            service.DeleteAll();

            productRepositoryMock.Received().DeleteAll();
        }
    }
}
