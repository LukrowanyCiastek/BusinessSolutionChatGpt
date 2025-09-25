using AutoFixture;
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
        public void Validate_ValueIIsNuLL_ReturnsError()
        {
            var expected = "Identyfiaktor musi być większy od 0";
            var validator = Fixture.Create<AddProductValidator>();

            var actual = validator.Validate(0);

            expected.Should().Be(actual.Errors.First().ErrorMessage);
        }

        [Test]
        public void Validate_ValueMoreThanZeroAndProductDoesNotExists_ReturnsError()
        {
            var shopCartManagerMock = Fixture.FreezeMock<IShopCartManager>();
            shopCartManagerMock.Exists(Arg.Any<long>()).Returns(false);
            var stringLocalizerMock = Fixture.FreezeMock<IStringLocalizer>();
            var expected = Fixture.Create<LocalizedString>();
            stringLocalizerMock.GetString("ProductNotExistValidationMessage").Returns(expected);
            var validator = Fixture.Create<ProductExistValidator>();

            var actual = validator.Validate(1);

            expected.Value.Should().Be(actual.Errors.First().ErrorMessage);
        }

        [Test]
        public void Validate_ValueMoreThanZeroAndProductExists_ValidationPass()
        {
            var shopCartManagerMock = Fixture.FreezeMock<IShopCartManager>();
            shopCartManagerMock.Exists(Arg.Any<long>()).Returns(true);
            var validator = Fixture.Create<ProductExistValidator>();

            var actual = validator.Validate(1);

            actual.IsValid.Should().BeTrue();
        }
    }
}
