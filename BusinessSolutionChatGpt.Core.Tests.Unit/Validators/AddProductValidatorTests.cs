using AutoFixture;
using BusinessSolutionChatGpt.Core.DTO.Product;
using BusinessSolutionChatGpt.Core.Interfaces;
using BusinessSolutionChatGpt.Core.Validators;
using BusinessSolutionChatGpt.Tests.Core;
using FluentAssertions;
using Microsoft.Extensions.Localization;
using NSubstitute;

namespace BusinessSolutionChatGpt.Core.Tests.Unit.Validators
{
    [TestFixture]
    public class AddProductValidatorTests : BaseFixture
    {
        [Test]
        public void Validate_ProductNameIsNull_ReturnsError()
        {
            var expected = Fixture.Create<LocalizedString>();
            var stringLocalizerMock = Fixture.FreezeMock<IStringLocalizer>();
            stringLocalizerMock.GetString("ProductMissingNameValidationMessage").Returns(expected);
            var product = Fixture.Create<AddProductDTO>();
            product.Name = null;
            var validator = Fixture.Create<AddProductValidator>();

            var actual = validator.Validate(product);

            expected.Value.Should().Be(actual.Errors.First().ErrorMessage);
        }

        [Test]
        public void Validate_ProductNameEmpty_ReturnsError()
        {
            var expected = Fixture.Create<LocalizedString>();
            var stringLocalizerMock = Fixture.FreezeMock<IStringLocalizer>();
            stringLocalizerMock.GetString("ProductEmptyNameValidationMessage").Returns(expected);
            var product = Fixture.Create<AddProductDTO>();
            product.Name = string.Empty;
            var validator = Fixture.Create<AddProductValidator>();

            var actual = validator.Validate(product);

            expected.Value.Should().Be(actual.Errors.First().ErrorMessage);
        }

        [Test]
        public void Validate_ProductPriceZero_ReturnsError()
        {
            var expected = Fixture.Create<LocalizedString>();
            var stringLocalizerMock = Fixture.FreezeMock<IStringLocalizer>();
            stringLocalizerMock.GetString("ProductNotPositivePriceValidationMessage").Returns(expected);
            var product = Fixture.Create<AddProductDTO>();
            product.Price = 0;
            var validator = Fixture.Create<AddProductValidator>();

            var actual = validator.Validate(product);

            expected.Value.Should().Be(actual.Errors.First().ErrorMessage);
        }

        [Test]
        public void Validate_ProductIsCorrect_ValidationPass()
        {
            var product = Fixture.Create<AddProductDTO>();
            product.Price = 1;
            var validator = Fixture.Create<AddProductValidator>();

            var actual = validator.Validate(product);

            actual.IsValid.Should().BeTrue();
        }
    }
}
