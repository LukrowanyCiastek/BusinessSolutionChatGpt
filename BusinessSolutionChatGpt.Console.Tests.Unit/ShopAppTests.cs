using AutoFixture;
using BusinessSolutionChatGpt.Console.Infrastructure.Interfaces;
using BusinessSolutionChatGpt.Core.DTO.Product;
using BusinessSolutionChatGpt.Core.Interfaces;
using BusinessSolutionChatGpt.Tests.Core;
using FluentValidation;
using Microsoft.Extensions.Localization;
using NSubstitute;
using NSubstitute.ReceivedExtensions;

namespace BusinessSolutionChatGpt.Console.Tests.Unit
{
    [TestFixture]
    public class ShopAppTests : BaseFixture
    {
        [Test]
        public void Start_AddProductAndStop_AddedProperOutputWithAddedProduct()
        {
            var mockOutput = Fixture.FreezeMock<IOutput>();
            var mockInput = Fixture.FreezeMock<IInput>();
            var mockValidator = Fixture.FreezeMock<IValidator<AddProductDTO>>();
            var mockShopCartManager = Fixture.FreezeMock<IShopCartManager>();
            var mockStringLocalizer = Fixture.FreezeMock<IStringLocalizer>();
            mockStringLocalizer.GetString("AddProductInstruction").Returns(Fixture.Create<LocalizedString>());
            mockStringLocalizer.GetString("ShowAllProductsInstruction").Returns(Fixture.Create<LocalizedString>());
            mockStringLocalizer.GetString("ShowTotalCostInstruction").Returns(Fixture.Create<LocalizedString>());
            mockStringLocalizer.GetString("RemoveSpecifiedProductInstruction").Returns(Fixture.Create<LocalizedString>());
            mockStringLocalizer.GetString("RemoveAllProductsInstruction").Returns(Fixture.Create<LocalizedString>());
            mockStringLocalizer.GetString("StopShopAppInstruction").Returns(Fixture.Create<LocalizedString>());
            var decimalOneKey = new ConsoleKeyInfo('1', ConsoleKey.D1, false, false, false);
            var escapeKey = new ConsoleKeyInfo('0', ConsoleKey.Escape, false, false, false);
            mockInput.ReadKey().Returns(decimalOneKey, escapeKey);
            var product = new AddProductDTO() { Name = "Woda", Price = 2.45m };
            mockInput.ReadObject(mockValidator, mockOutput).Returns(product);

            var app = Fixture.Create<ShopApp>();

            app.Start();

            mockOutput.Received().WriteLine(string.Empty);
            mockOutput.Received().WriteLine(mockStringLocalizer.GetString("AddProductInstruction").Value);
            mockOutput.Received().WriteLine(mockStringLocalizer.GetString("ShowAllProductsInstruction").Value);
            mockOutput.Received().WriteLine(mockStringLocalizer.GetString("ShowTotalCostInstruction").Value);
            mockOutput.Received().WriteLine(mockStringLocalizer.GetString("RemoveSpecifiedProductInstruction").Value);
            mockOutput.Received().WriteLine(mockStringLocalizer.GetString("RemoveAllProductsInstruction").Value);
            mockOutput.Received().WriteLine(mockStringLocalizer.GetString("StopShopAppInstruction").Value);
            mockShopCartManager.Received().Add(product);
            mockOutput.Received().WriteLine(string.Empty);
            mockOutput.Received().WriteLine(mockStringLocalizer.GetString("AddProductInstruction").Value);
            mockOutput.Received().WriteLine(mockStringLocalizer.GetString("ShowAllProductsInstruction").Value);
            mockOutput.Received().WriteLine(mockStringLocalizer.GetString("ShowTotalCostInstruction").Value);
            mockOutput.Received().WriteLine(mockStringLocalizer.GetString("RemoveSpecifiedProductInstruction").Value);
            mockOutput.Received().WriteLine(mockStringLocalizer.GetString("RemoveAllProductsInstruction").Value);
            mockOutput.Received().WriteLine(mockStringLocalizer.GetString("StopShopAppInstruction").Value);
        }
    }
}
