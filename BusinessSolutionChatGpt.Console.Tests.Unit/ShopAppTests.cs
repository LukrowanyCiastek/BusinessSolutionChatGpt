using AutoFixture;
using BusinessSolutionChatGpt.Console.Infrastructure.Interfaces;
using BusinessSolutionChatGpt.Core.DTO.Product;
using BusinessSolutionChatGpt.Core.Interfaces;
using BusinessSolutionChatGpt.Core.Validators;
using BusinessSolutionChatGpt.Tests.Core;
using FluentValidation;
using Microsoft.Extensions.Localization;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using System.Globalization;

namespace BusinessSolutionChatGpt.Console.Tests.Unit
{
    [TestFixture]
    public class ShopAppTests : BaseFixture
    {
        private const string AddProductInstructionKey = "AddProductInstruction";
        private const string ShowAllProductsInstructionKey = "ShowAllProductsInstruction";
        private const string ShowTotalCostInstructionKey = "ShowTotalCostInstruction";
        private const string RemoveSpecifiedProductInstructionKey = "RemoveSpecifiedProductInstruction";
        private const string RemoveAllProductsInstructionKey = "RemoveAllProductsInstruction";
        private const string StopShopAppInstructionKey = "StopShopAppInstruction";

        public void InitializeBaseInstructions(IStringLocalizer localizer)
        {
            localizer.GetString(AddProductInstructionKey).Returns(Fixture.Create<LocalizedString>());
            localizer.GetString(ShowAllProductsInstructionKey).Returns(Fixture.Create<LocalizedString>());
            localizer.GetString(ShowTotalCostInstructionKey).Returns(Fixture.Create<LocalizedString>());
            localizer.GetString(RemoveSpecifiedProductInstructionKey).Returns(Fixture.Create<LocalizedString>());
            localizer.GetString(RemoveAllProductsInstructionKey).Returns(Fixture.Create<LocalizedString>());
            localizer.GetString(StopShopAppInstructionKey).Returns(Fixture.Create<LocalizedString>());
        }

        public void VerifyPrintInstructions(IStringLocalizer localizer, IOutput output)
        {
            output.Received().WriteLine(string.Empty);
            output.Received().WriteLine(localizer.GetString(AddProductInstructionKey).Value);
            output.Received().WriteLine(localizer.GetString(ShowAllProductsInstructionKey).Value);
            output.Received().WriteLine(localizer.GetString(ShowTotalCostInstructionKey).Value);
            output.Received().WriteLine(localizer.GetString(RemoveSpecifiedProductInstructionKey).Value);
            output.Received().WriteLine(localizer.GetString(RemoveAllProductsInstructionKey).Value);
            output.Received().WriteLine(localizer.GetString(StopShopAppInstructionKey).Value);
        }

        [Test]
        public void Start_AddProductAndStop_AddedProperOutputWithAddedProduct()
        {
            var mockOutput = Fixture.FreezeMock<IOutput>();
            var mockInput = Fixture.FreezeMock<IInput>();
            var mockValidator = Fixture.FreezeMock<IValidator<AddProductDTO>>();
            var mockShopCartManager = Fixture.FreezeMock<IShopCartManager>();
            var mockStringLocalizer = Fixture.FreezeMock<IStringLocalizer>();
            InitializeBaseInstructions(mockStringLocalizer);
            var decimalOneKey = new ConsoleKeyInfo('1', ConsoleKey.D1, false, false, false);
            var escapeKey = new ConsoleKeyInfo('0', ConsoleKey.Escape, false, false, false);
            mockInput.ReadKey().Returns(decimalOneKey, escapeKey);
            var product = new AddProductDTO() { Name = "Woda", Price = 2.45m };
            mockInput.ReadObject(mockValidator, mockOutput).Returns(product);

            var app = Fixture.Create<ShopApp>();

            app.Start();

            VerifyPrintInstructions(mockStringLocalizer, mockOutput);
            mockShopCartManager.Received().Add(product);
            VerifyPrintInstructions(mockStringLocalizer, mockOutput);
        }

        [Test]
        public void Start_ProductsAreEmptyPrintAndStop_PrintedProducts()
        {
            var mockOutput = Fixture.FreezeMock<IOutput>();
            var mockInput = Fixture.FreezeMock<IInput>();
            var mockShopCartManager = Fixture.FreezeMock<IShopCartManager>();
            mockShopCartManager.GetAll().Returns(new List<ProductDetailsDTO>());
            var mockStringLocalizer = Fixture.FreezeMock<IStringLocalizer>();
            InitializeBaseInstructions(mockStringLocalizer);
            var decimalOneKey = new ConsoleKeyInfo('2', ConsoleKey.D2, false, false, false);
            var escapeKey = new ConsoleKeyInfo('0', ConsoleKey.Escape, false, false, false);
            mockInput.ReadKey().Returns(decimalOneKey, escapeKey);

            var app = Fixture.Create<ShopApp>();

            app.Start();

            VerifyPrintInstructions(mockStringLocalizer, mockOutput);
            mockOutput.Received().WriteLine(string.Empty);
            mockOutput.Received().WriteLine("koszyk jest pusty");
            VerifyPrintInstructions(mockStringLocalizer, mockOutput);
        }

        [Test]
        public void Start_ProductsAreExistPrintAndStop_PrintedProducts()
        {
            var mockOutput = Fixture.FreezeMock<IOutput>();
            var mockInput = Fixture.FreezeMock<IInput>();
            var mockShopCartManager = Fixture.FreezeMock<IShopCartManager>();
            var product = new ProductDetailsDTO { Name = Fixture.Create<string>(), Price = Fixture.Create<decimal>() };
            mockShopCartManager.GetAll().Returns(new List<ProductDetailsDTO> { product });
            var mockStringLocalizer = Fixture.FreezeMock<IStringLocalizer>();
            InitializeBaseInstructions(mockStringLocalizer);
            var decimalOneKey = new ConsoleKeyInfo('2', ConsoleKey.D2, false, false, false);
            var escapeKey = new ConsoleKeyInfo('0', ConsoleKey.Escape, false, false, false);
            mockInput.ReadKey().Returns(decimalOneKey, escapeKey);

            var app = Fixture.Create<ShopApp>();

            app.Start();

            VerifyPrintInstructions(mockStringLocalizer, mockOutput);
            mockOutput.Received().WriteLine(string.Empty);
            mockOutput.Received().WriteLine($"Produkt 1");
            mockOutput.Received().WriteLine($"Nazwa {product.Name}");
            mockOutput.Received().WriteLine($"Cena {product.Price.ToString(CultureInfo.InvariantCulture)}");
            VerifyPrintInstructions(mockStringLocalizer, mockOutput);
        }

        [Test]
        public void Start_GetTotalCostAndStop_PrintTotalCost()
        {
            var mockOutput = Fixture.FreezeMock<IOutput>();
            var mockInput = Fixture.FreezeMock<IInput>();
            var mockShopCartManager = Fixture.FreezeMock<IShopCartManager>();
            var cost = Fixture.Create<decimal>();
            mockShopCartManager.GetTotalCost().Returns(cost);
            var mockStringLocalizer = Fixture.FreezeMock<IStringLocalizer>();
            InitializeBaseInstructions(mockStringLocalizer);
            var decimalOneKey = new ConsoleKeyInfo('3', ConsoleKey.D3, false, false, false);
            var escapeKey = new ConsoleKeyInfo('0', ConsoleKey.Escape, false, false, false);
            mockInput.ReadKey().Returns(decimalOneKey, escapeKey);

            var app = Fixture.Create<ShopApp>();

            app.Start();

            VerifyPrintInstructions(mockStringLocalizer, mockOutput);
            mockOutput.Received().WriteLineWithEscape($"Całkowity koszt to: {cost.ToString(CultureInfo.InvariantCulture)}");
            VerifyPrintInstructions(mockStringLocalizer, mockOutput);
        }

        [Test]
        public void Start_DeleteProductAndStop_ProductDeleted()
        {
            var mockOutput = Fixture.FreezeMock<IOutput>();
            var mockInput = Fixture.FreezeMock<IInput>();
            var mockShopCartManager = Fixture.FreezeMock<IShopCartManager>();
            var mockStringLocalizer = Fixture.FreezeMock<IStringLocalizer>();
            var validator = new ProductExistValidator(mockStringLocalizer, mockShopCartManager);
            Fixture.Inject(validator);
            InitializeBaseInstructions(mockStringLocalizer);
            var localizedString = Fixture.Create<LocalizedString>();
            mockStringLocalizer.GetString("ProductIdentifierInstruction").Returns(localizedString);
            var decimalOneKey = new ConsoleKeyInfo('4', ConsoleKey.D4, false, false, false);
            var escapeKey = new ConsoleKeyInfo('0', ConsoleKey.Escape, false, false, false);
            mockInput.ReadKey().Returns(decimalOneKey, escapeKey);
            var identifier = 1;
            mockInput.ReadPrimitive(validator, mockOutput, localizedString.Value).Returns(identifier);

            var app = Fixture.Create<ShopApp>();

            app.Start();

            VerifyPrintInstructions(mockStringLocalizer, mockOutput);
            mockShopCartManager.Received().Delete(identifier - 1);
            mockOutput.WriteLine($"Usunięto produkt o identyfikatorze: {identifier}");
            VerifyPrintInstructions(mockStringLocalizer, mockOutput);
        }

        [Test]
        public void Start_DeleteAllProductAndStop_AllProductDeleted()
        {
            var mockOutput = Fixture.FreezeMock<IOutput>();
            var mockInput = Fixture.FreezeMock<IInput>();
            var mockShopCartManager = Fixture.FreezeMock<IShopCartManager>();
            var mockStringLocalizer = Fixture.FreezeMock<IStringLocalizer>();
            InitializeBaseInstructions(mockStringLocalizer);
            var localizedString = Fixture.Create<LocalizedString>();
            var decimalOneKey = new ConsoleKeyInfo('5', ConsoleKey.D5, false, false, false);
            var escapeKey = new ConsoleKeyInfo('0', ConsoleKey.Escape, false, false, false);
            mockInput.ReadKey().Returns(decimalOneKey, escapeKey);
            var identifier = Fixture.Create<long>();

            var app = Fixture.Create<ShopApp>();

            app.Start();

            VerifyPrintInstructions(mockStringLocalizer, mockOutput);
            mockShopCartManager.Received().DeleteAll();
            mockOutput.WriteLineWithEscape($"Koszyk został wyczyszczony");
            VerifyPrintInstructions(mockStringLocalizer, mockOutput);
        }

        [Test]
        public void Start_GiveWrongInstruction_PrintProperMessage()
        {
            var mockOutput = Fixture.FreezeMock<IOutput>();
            var mockInput = Fixture.FreezeMock<IInput>();
            var mockStringLocalizer = Fixture.FreezeMock<IStringLocalizer>();
            InitializeBaseInstructions(mockStringLocalizer);
            var localizedString = Fixture.Create<LocalizedString>();
            var decimalOneKey = new ConsoleKeyInfo('6', ConsoleKey.D6, false, false, false);
            var escapeKey = new ConsoleKeyInfo('0', ConsoleKey.Escape, false, false, false);
            mockInput.ReadKey().Returns(decimalOneKey, escapeKey);
            var identifier = Fixture.Create<long>();

            var app = Fixture.Create<ShopApp>();

            app.Start();

            VerifyPrintInstructions(mockStringLocalizer, mockOutput);
            mockOutput.WriteLineWithEscape("niepoprawna komenda");
            VerifyPrintInstructions(mockStringLocalizer, mockOutput);
        }
    }
}
