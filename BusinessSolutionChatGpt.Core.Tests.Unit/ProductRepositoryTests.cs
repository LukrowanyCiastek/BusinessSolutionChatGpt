using AutoFixture;
using BusinessSolutionChatGpt.Core.Interfaces;
using BusinessSolutionChatGpt.Core.Model;
using BusinessSolutionChatGpt.Tests.Core;
using FluentAssertions;

namespace BusinessSolutionChatGpt.Core.Tests.Unit
{
    [TestFixture]
    public class ProductRepositoryTests : BaseFixture
    {
        [Test]
        public void Add_Product_RepositoryIsNotEmptyAndContainsGivenObject()
        {
            var expected = Fixture.Create<Product>();
            var dataSet = new List<Product>();
            Fixture.Inject<IList<Product>>(dataSet);
            IProductRepository repository = Fixture.Create<ProductRepository>();

            repository.Add(expected);

            dataSet.Should().NotBeEmpty();
            dataSet.Should().Contain(expected);
        }

        [Test]
        public void Add_ProductIsNull_ThrowsArgumentNullException()
        {
            Action act = () =>
            {
                IProductRepository repository = Fixture.Create<ProductRepository>();
                repository.Add(null);
            };

            act.Should()
               .Throw<ArgumentNullException>()
               .WithMessage("Product can't be null (Parameter 'product')");
        }

        [Test]
        public void DeleteForEmptyRepository_ProductDoesNotExist_NotDeleteProduct()
        {
            var dataSet = new List<Product>();
            Fixture.Inject<IList<Product>>(dataSet);
            IProductRepository repository = Fixture.Create<ProductRepository>();

            repository.Delete(Fixture.Create<long>());

            dataSet.Should().BeEmpty();
        }

        [Test]
        public void DeleteForNotEmpty_ProductDoesNotExist_NotDeleteProduct()
        {
            var dataSet = new List<Product>();
            Fixture.Inject<IList<Product>>(dataSet);
            IProductRepository repository = Fixture.Create<ProductRepository>();
            repository.Add(Fixture.Create<Product>());

            repository.Delete(2);

            dataSet.Should().NotBeEmpty();
            dataSet.Count.Should().Be(1);
        }

        [Test]
        public void DeleteForNotEmpty_ProductExist_DeleteProduct()
        {
            var dataSet = new List<Product>();
            Fixture.Inject<IList<Product>>(dataSet);
            IProductRepository repository = Fixture.Create<ProductRepository>();
            repository.Add(Fixture.Create<Product>());

            repository.Delete(0);

            dataSet.Should().BeEmpty();
        }

        [Test]
        public void DeleteAll_EmptyReposotory_RepositoryIsEmpty()
        {
            var dataSet = new List<Product>();
            Fixture.Inject<IList<Product>>(dataSet);
            IProductRepository repository = Fixture.Create<ProductRepository>();

            repository.DeleteAll();

            dataSet.Should().BeEmpty();
        }

        [Test]
        public void DeleteAll_ReposotoryContainsProducts_RepositoryIsEmpty()
        {
            var dataSet = new List<Product>();
            Fixture.Inject<IList<Product>>(dataSet);
            IProductRepository repository = Fixture.Create<ProductRepository>();
            repository.Add(Fixture.Create<Product>());

            repository.DeleteAll();

            dataSet.Should().BeEmpty();
        }

        [Test]
        public void Exists_IndexBelowZero_ReturnsFalse()
        {
            IProductRepository repository = Fixture.Create<ProductRepository>();

            var actual = repository.Exists(-1);

            actual.Should().BeFalse();
        }

        [Test]
        public void Exists_IndexAboveRepositorySize_ReturnsFalse()
        {
            var dataSet = new List<Product>();
            Fixture.Inject<IList<Product>>(dataSet);
            IProductRepository repository = Fixture.Create<ProductRepository>();

            var actual = repository.Exists(1);

            actual.Should().BeFalse();
        }

        [Test]
        public void GetAll_RepositoryWithProducts_ReturnsAllProducts()
        {
            var addedProduct = Fixture.Create<Product>();
            var dataSet = new List<Product>();
            Fixture.Inject<IList<Product>>(dataSet);
            IProductRepository repository = Fixture.Create<ProductRepository>();
            repository.Add(addedProduct);
            
            var actual = repository.GetAll();

            actual.Should().BeEquivalentTo(dataSet);
        }

        [Test]
        public void GetTotalPrice_RepositoryIsEmpty_Returns0()
        {
            var addedProduct = Fixture.Create<Product>();
            var dataSet = new List<Product>();
            Fixture.Inject<IList<Product>>(dataSet);
            IProductRepository repository = Fixture.Create<ProductRepository>();

            var actual = repository.GetTotalPrice();

            actual.Should().Be(0);
        }

        [Test]
        public void GetTotalPrice_RepositoryConatiansProducts_ReturnsPriceOfAllProducts()
        {
            var addedProduct = Fixture.Create<Product>();
            var dataSet = new List<Product>();
            Fixture.Inject<IList<Product>>(dataSet);
            IProductRepository repository = Fixture.Create<ProductRepository>();
            repository.Add(addedProduct);

            var actual = repository.GetTotalPrice();

            actual.Should().Be(addedProduct.Price);
        }
    }
}