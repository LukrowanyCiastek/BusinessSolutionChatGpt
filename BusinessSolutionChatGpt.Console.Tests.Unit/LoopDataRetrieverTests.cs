using AutoFixture;
using BusinessSolutionChatGpt.Console.Infrastructure.Interfaces;
using BusinessSolutionChatGpt.Console.Interfaces;
using BusinessSolutionChatGpt.Console.Tests.Unit.Utils;
using BusinessSolutionChatGpt.Tests.Core;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using NSubstitute;
using Spectre.Console;
using System.Globalization;

namespace BusinessSolutionChatGpt.Console.Tests.Unit
{
    [TestFixture]
    public class LoopDataRetrieverTests : BaseFixture
    {
        [Test]
        public void ReadPrimitive_GiveNotPrimitive_ThrowsInvalidOperationException()
        {
            var mockOutput = Fixture.FreezeMock<IOutput>();
            Action act = () =>
            {
                ILoopDataRetriever<object> retriever = Fixture.Create<LoopDataRetriever<object>>();
                retriever.ReadPrimitive(null, mockOutput, Fixture.Create<string>());
            };

            act.Should()
               .Throw<InvalidOperationException>()
               .WithMessage("Type is object primitive");
        }

        [TestCase("true", false, true)]
        [TestCase("false", false, false)]
        [TestCase("true", true, true)]
        [TestCase("false", true, false)]
        [TestCase("", true, null)]
        public void ReadPrimitiveBool_MissingValidator_ReturnsReadedValue(string readedText, bool allowedNull, bool? expected)
        {
            var mockOutput = Fixture.FreezeMock<IOutput>();
            var mockPrompt = Fixture.FreezeMock<IPrompt<string>>();
            var mockPromptFactory = Fixture.FreezeMock<IPromptFactory>();
            mockPrompt.Show(Arg.Any<IAnsiConsole>()).Returns(readedText);
            mockPromptFactory.CreateSelectionPrompt(Arg.Any<string>(), Arg.Any<object>(), allowedNull).Returns(mockPrompt);
            ILoopDataRetriever<bool?> retriever = Fixture.Create<LoopDataRetriever<bool?>>();
            
            bool? actual = retriever.ReadPrimitive(null, mockOutput, Fixture.Create<string>());

            actual.Should().Be(expected);
        }

        [TestCase("true", false, true)]
        [TestCase("false", false, false)]
        [TestCase("true", true, true)]
        [TestCase("false", true, false)]
        [TestCase("", true, null)]
        public void ReadPrimitiveBool_GiveValidatorPassData_ReturnsReadedValue(string readedText, bool allowedNull, bool? expected)
        {
            var mockOutput = Fixture.FreezeMock<IOutput>();
            var mockValidator = Fixture.FreezeMock<IValidator<bool?>>();
            var succesValidation = new FluentValidation.Results.ValidationResult();
            mockValidator.Validate(expected).Returns(succesValidation);
            var prompt = Fixture.FreezeMock<IPrompt<string>>();
            var promptFactory = Fixture.FreezeMock<IPromptFactory>();

            prompt.Show(Arg.Any<IAnsiConsole>()).Returns(readedText);
            promptFactory.CreateSelectionPrompt(Arg.Any<string>(), Arg.Any<object>(), allowedNull).Returns(prompt);
            ILoopDataRetriever<bool?> retriever = Fixture.Create<LoopDataRetriever<bool?>>();

            bool? actual = retriever.ReadPrimitive(mockValidator, mockOutput, Fixture.Create<string>());

            actual.Should().Be(expected);
        }

        [TestCase("true", false, true)]
        [TestCase("false", false, false)]
        [TestCase("true", true, true)]
        [TestCase("false", true, false)]
        [TestCase("", true, null)]
        public void ReadPrimitiveBool_GiveValidatorNotPassData_ChecksPrintErrors(string readedText, bool allowedNull, bool? expected)
        {
            var mockOutput = Fixture.FreezeMock<IOutput>();
            var mockValidator = Fixture.FreezeMock<IValidator<bool?>>();
            var propertyNameError = Fixture.FreezeMock<string>();
            var errorMessage = Fixture.FreezeMock<string>();
            var faliedValidation = new FluentValidation.Results.ValidationResult();
            faliedValidation.Errors.Add(new ValidationFailure(propertyNameError, errorMessage));
            var succesValidation = new FluentValidation.Results.ValidationResult();
            mockValidator.Validate(expected).Returns(faliedValidation, succesValidation);
            var prompt = Fixture.FreezeMock<IPrompt<string>>();
            var promptFactory = Fixture.FreezeMock<IPromptFactory>();

            prompt.Show(Arg.Any<IAnsiConsole>()).Returns(readedText);
            promptFactory.CreateSelectionPrompt(Arg.Any<string>(), Arg.Any<object>(), allowedNull).Returns(prompt);
            ILoopDataRetriever<bool?> retriever = Fixture.Create<LoopDataRetriever<bool?>>();

            retriever.ReadPrimitive(mockValidator, mockOutput, Fixture.Create<string>());

            mockOutput.WriteLineWithEscape(errorMessage);
        }

        [TestCase("None", false, TestEnum.None)]
        [TestCase("None", true, TestEnum.None)]
        [TestCase("", true, null)]
        public void ReadPrimitiveEnum_MissingValidator_ReturnsReadedValue(string readedText, bool allowedNull, TestEnum? expected)
        {
            var mockOutput = Fixture.FreezeMock<IOutput>();
            var mockPrompt = Fixture.FreezeMock<IPrompt<string>>();
            var mockPromptFactory = Fixture.FreezeMock<IPromptFactory>();
            mockPrompt.Show(Arg.Any<IAnsiConsole>()).Returns(readedText);
            mockPromptFactory.CreateSelectionPrompt(Arg.Any<string>(), Arg.Any<object>(), allowedNull).Returns(mockPrompt);
            ILoopDataRetriever<TestEnum?> retriever = Fixture.Create<LoopDataRetriever<TestEnum?>>();

            TestEnum? actual = retriever.ReadPrimitive(null, mockOutput, Fixture.Create<string>());

            actual.Should().Be(expected);
        }

        [TestCase("None", false, TestEnum.None)]
        [TestCase("None", true, TestEnum.None)]
        [TestCase("", true, null)]
        public void ReadPrimitiveEnum_GiveValidatorPassData_ReturnsReadedValue(string readedText, bool allowedNull, TestEnum? expected)
        {
            var mockOutput = Fixture.FreezeMock<IOutput>();
            var mockValidator = Fixture.FreezeMock<IValidator<TestEnum?>>();
            var succesValidation = new FluentValidation.Results.ValidationResult();
            mockValidator.Validate(expected).Returns(succesValidation);
            var prompt = Fixture.FreezeMock<IPrompt<string>>();
            var promptFactory = Fixture.FreezeMock<IPromptFactory>();

            prompt.Show(Arg.Any<IAnsiConsole>()).Returns(readedText);
            promptFactory.CreateSelectionPrompt(Arg.Any<string>(), Arg.Any<object>(), allowedNull).Returns(prompt);
            ILoopDataRetriever<TestEnum?> retriever = Fixture.Create<LoopDataRetriever<TestEnum?>>();

            TestEnum? actual = retriever.ReadPrimitive(mockValidator, mockOutput, Fixture.Create<string>());

            actual.Should().Be(expected);
        }

        [TestCase("None", false, TestEnum.None)]
        [TestCase("None", true, TestEnum.None)]
        [TestCase("", true, null)]
        public void ReadPrimitiveEnum_GiveValidatorNotPassData_ChecksPrintErrors(string readedText, bool allowedNull, TestEnum? expected)
        {
            var mockOutput = Fixture.FreezeMock<IOutput>();
            var mockValidator = Fixture.FreezeMock<IValidator<TestEnum?>>();
            var propertyNameError = Fixture.FreezeMock<string>();
            var errorMessage = Fixture.FreezeMock<string>();
            var faliedValidation = new FluentValidation.Results.ValidationResult();
            faliedValidation.Errors.Add(new ValidationFailure(propertyNameError, errorMessage));
            var succesValidation = new FluentValidation.Results.ValidationResult();
            mockValidator.Validate(expected).Returns(faliedValidation, succesValidation);
            var prompt = Fixture.FreezeMock<IPrompt<string>>();
            var promptFactory = Fixture.FreezeMock<IPromptFactory>();

            prompt.Show(Arg.Any<IAnsiConsole>()).Returns(readedText);
            promptFactory.CreateSelectionPrompt(Arg.Any<string>(), Arg.Any<object>(), allowedNull).Returns(prompt);
            ILoopDataRetriever<TestEnum?> retriever = Fixture.Create<LoopDataRetriever<TestEnum?>>();

            retriever.ReadPrimitive(mockValidator, mockOutput, Fixture.Create<string>());

            mockOutput.WriteLineWithEscape(errorMessage);
        }

        [TestCase("1990-01-25", false)]
        [TestCase("1990-01-25", true)]
        [TestCase("", true)]
        public void ReadPrimitiveDateTime_MissingValidator_ReturnsReadedValue(string readedText, bool allowedNull)
        {
            DateTime? expected = string.IsNullOrEmpty(readedText) ? null : DateTime.Parse(readedText, CultureInfo.InvariantCulture);
            var mockOutput = Fixture.FreezeMock<IOutput>();
            var mockPrompt = Fixture.FreezeMock<IPrompt<string>>();
            var mockPromptFactory = Fixture.FreezeMock<IPromptFactory>();
            mockPrompt.Show(Arg.Any<IAnsiConsole>()).Returns(readedText);
            mockPromptFactory.CreateTextPrompt(Arg.Any<string>(), Arg.Any<object>(), allowedNull).Returns(mockPrompt);
            ILoopDataRetriever<DateTime?> retriever = Fixture.Create<LoopDataRetriever<DateTime?>>();

            DateTime? actual = retriever.ReadPrimitive(null, mockOutput, Fixture.Create<string>());

            actual.Should().Be(expected);
        }

        [TestCase("1990-01-25", false)]
        [TestCase("1990-01-25", true)]
        [TestCase("", true)]
        public void ReadPrimitiveDateTime_GiveValidatorPassData_ReturnsReadedValue(string readedText, bool allowedNull)
        {
            DateTime? expected = string.IsNullOrEmpty(readedText) ? null : DateTime.Parse(readedText, CultureInfo.InvariantCulture);
            var mockOutput = Fixture.FreezeMock<IOutput>();
            var mockValidator = Fixture.FreezeMock<IValidator<DateTime?>>();
            var succesValidation = new FluentValidation.Results.ValidationResult();
            mockValidator.Validate(expected).Returns(succesValidation);
            var prompt = Fixture.FreezeMock<IPrompt<string>>();
            var promptFactory = Fixture.FreezeMock<IPromptFactory>();

            prompt.Show(Arg.Any<IAnsiConsole>()).Returns(readedText);
            promptFactory.CreateTextPrompt(Arg.Any<string>(), Arg.Any<object>(), allowedNull).Returns(prompt);
            ILoopDataRetriever<DateTime?> retriever = Fixture.Create<LoopDataRetriever<DateTime?>>();

            DateTime? actual = retriever.ReadPrimitive(mockValidator, mockOutput, Fixture.Create<string>());

            actual.Should().Be(expected);
        }

        [TestCase("1990-01-25", false)]
        [TestCase("1990-01-25", true)]
        [TestCase("", true)]
        public void ReadPrimitiveDateTime_GiveValidatorNotPassData_ChecksPrintErrors(string readedText, bool allowedNull)
        {
            DateTime? expected = string.IsNullOrEmpty(readedText) ? null : DateTime.Parse(readedText, CultureInfo.InvariantCulture);
            var mockOutput = Fixture.FreezeMock<IOutput>();
            var mockValidator = Fixture.FreezeMock<IValidator<DateTime?>>();
            var propertyNameError = Fixture.FreezeMock<string>();
            var errorMessage = Fixture.FreezeMock<string>();
            var faliedValidation = new FluentValidation.Results.ValidationResult();
            faliedValidation.Errors.Add(new ValidationFailure(propertyNameError, errorMessage));
            var succesValidation = new FluentValidation.Results.ValidationResult();
            mockValidator.Validate(expected).Returns(faliedValidation, succesValidation);
            var prompt = Fixture.FreezeMock<IPrompt<string>>();
            var promptFactory = Fixture.FreezeMock<IPromptFactory>();

            prompt.Show(Arg.Any<IAnsiConsole>()).Returns(readedText);
            promptFactory.CreateTextPrompt(Arg.Any<string>(), Arg.Any<object>(), allowedNull).Returns(prompt);
            ILoopDataRetriever<DateTime?> retriever = Fixture.Create<LoopDataRetriever<DateTime?>>();

            retriever.ReadPrimitive(mockValidator, mockOutput, Fixture.Create<string>());

            mockOutput.WriteLineWithEscape(errorMessage);
        }

        [TestCase("00000000-0000-0000-0000-000000000001", false)]
        [TestCase("00000000-0000-0000-0000-000000000001", true)]
        [TestCase("", true)]
        public void ReadPrimitiveGuid_MissingValidator_ReturnsReadedValue(string readedText, bool allowedNull)
        {
            Guid? expected = string.IsNullOrEmpty(readedText) ? null : Guid.Parse(readedText);
            var mockOutput = Fixture.FreezeMock<IOutput>();
            var mockPrompt = Fixture.FreezeMock<IPrompt<string>>();
            var mockPromptFactory = Fixture.FreezeMock<IPromptFactory>();
            mockPrompt.Show(Arg.Any<IAnsiConsole>()).Returns(readedText);
            mockPromptFactory.CreateTextPrompt(Arg.Any<string>(), Arg.Any<object>(), allowedNull).Returns(mockPrompt);
            ILoopDataRetriever<Guid?> retriever = Fixture.Create<LoopDataRetriever<Guid?>>();

            Guid? actual = retriever.ReadPrimitive(null, mockOutput, Fixture.Create<string>());

            actual.Should().Be(expected);
        }

        [TestCase("00000000-0000-0000-0000-000000000001", false)]
        [TestCase("00000000-0000-0000-0000-000000000001", true)]
        [TestCase("", true)]
        public void ReadPrimitiveGuid_GiveValidatorPassData_ReturnsReadedValue(string readedText, bool allowedNull)
        {
            Guid? expected = string.IsNullOrEmpty(readedText) ? null : Guid.Parse(readedText);
            var mockOutput = Fixture.FreezeMock<IOutput>();
            var mockValidator = Fixture.FreezeMock<IValidator<Guid?>>();
            var succesValidation = new FluentValidation.Results.ValidationResult();
            mockValidator.Validate(expected).Returns(succesValidation);
            var prompt = Fixture.FreezeMock<IPrompt<string>>();
            var promptFactory = Fixture.FreezeMock<IPromptFactory>();

            prompt.Show(Arg.Any<IAnsiConsole>()).Returns(readedText);
            promptFactory.CreateTextPrompt(Arg.Any<string>(), Arg.Any<object>(), allowedNull).Returns(prompt);
            ILoopDataRetriever<Guid?> retriever = Fixture.Create<LoopDataRetriever<Guid?>> ();

            Guid? actual = retriever.ReadPrimitive(mockValidator, mockOutput, Fixture.Create<string>());

            actual.Should().Be(expected);
        }

        [TestCase("00000000-0000-0000-0000-000000000001", false)]
        [TestCase("00000000-0000-0000-0000-000000000001", true)]
        [TestCase("", true)]
        public void ReadPrimitiveGuid_GiveValidatorNotPassData_ChecksPrintErrors(string readedText, bool allowedNull)
        {
            Guid? expected = string.IsNullOrEmpty(readedText) ? null : Guid.Parse(readedText);
            var mockOutput = Fixture.FreezeMock<IOutput>();
            var mockValidator = Fixture.FreezeMock<IValidator<Guid?>>();
            var propertyNameError = Fixture.FreezeMock<string>();
            var errorMessage = Fixture.FreezeMock<string>();
            var faliedValidation = new FluentValidation.Results.ValidationResult();
            faliedValidation.Errors.Add(new ValidationFailure(propertyNameError, errorMessage));
            var succesValidation = new FluentValidation.Results.ValidationResult();
            mockValidator.Validate(expected).Returns(faliedValidation, succesValidation);
            var prompt = Fixture.FreezeMock<IPrompt<string>>();
            var promptFactory = Fixture.FreezeMock<IPromptFactory>();

            prompt.Show(Arg.Any<IAnsiConsole>()).Returns(readedText);
            promptFactory.CreateTextPrompt(Arg.Any<string>(), Arg.Any<object>(), allowedNull).Returns(prompt);
            ILoopDataRetriever<Guid?> retriever = Fixture.Create<LoopDataRetriever<Guid?>>();

            retriever.ReadPrimitive(mockValidator, mockOutput, Fixture.Create<string>());

            mockOutput.WriteLineWithEscape(errorMessage);
        }

        [TestCase("1", false, 1)]
        [TestCase("1", true, 1)]
        [TestCase("", true, null)]
        public void ReadPrimitiveInt_MissingValidator_ReturnsReadedValue(string readedText, bool allowedNull, int? expected)
        {
            var mockOutput = Fixture.FreezeMock<IOutput>();
            var mockPrompt = Fixture.FreezeMock<IPrompt<string>>();
            var mockPromptFactory = Fixture.FreezeMock<IPromptFactory>();
            mockPrompt.Show(Arg.Any<IAnsiConsole>()).Returns(readedText);
            mockPromptFactory.CreateTextPrompt(Arg.Any<string>(), Arg.Any<object>(), allowedNull).Returns(mockPrompt);
            ILoopDataRetriever<int?> retriever = Fixture.Create<LoopDataRetriever<int?>>();

            int? actual = retriever.ReadPrimitive(null, mockOutput, Fixture.Create<string>());

            actual.Should().Be(expected);
        }

        [TestCase("1", false, 1)]
        [TestCase("1", true, 1)]
        [TestCase("", true, null)]
        public void ReadPrimitiveInt_GiveValidatorPassData_ReturnsReadedValue(string readedText, bool allowedNull, int? expected)
        {
            var mockOutput = Fixture.FreezeMock<IOutput>();
            var mockValidator = Fixture.FreezeMock<IValidator<int?>>();
            var succesValidation = new FluentValidation.Results.ValidationResult();
            mockValidator.Validate(expected).Returns(succesValidation);
            var prompt = Fixture.FreezeMock<IPrompt<string>>();
            var promptFactory = Fixture.FreezeMock<IPromptFactory>();

            prompt.Show(Arg.Any<IAnsiConsole>()).Returns(readedText);
            promptFactory.CreateTextPrompt(Arg.Any<string>(), Arg.Any<object>(), allowedNull).Returns(prompt);
            ILoopDataRetriever<int?> retriever = Fixture.Create<LoopDataRetriever<int?>>();

            int? actual = retriever.ReadPrimitive(mockValidator, mockOutput, Fixture.Create<string>());

            actual.Should().Be(expected);
        }

        [TestCase("1", false, 1)]
        [TestCase("1", true, 1)]
        [TestCase("", true, null)]
        public void ReadPrimitiveInt_GiveValidatorNotPassData_ChecksPrintErrors(string readedText, bool allowedNull, int? expected)
        {
            var mockOutput = Fixture.FreezeMock<IOutput>();
            var mockValidator = Fixture.FreezeMock<IValidator<int?>>();
            var propertyNameError = Fixture.FreezeMock<string>();
            var errorMessage = Fixture.FreezeMock<string>();
            var faliedValidation = new FluentValidation.Results.ValidationResult();
            faliedValidation.Errors.Add(new ValidationFailure(propertyNameError, errorMessage));
            var succesValidation = new FluentValidation.Results.ValidationResult();
            mockValidator.Validate(expected).Returns(faliedValidation, succesValidation);
            var prompt = Fixture.FreezeMock<IPrompt<string>>();
            var promptFactory = Fixture.FreezeMock<IPromptFactory>();

            prompt.Show(Arg.Any<IAnsiConsole>()).Returns(readedText);
            promptFactory.CreateTextPrompt(Arg.Any<string>(), Arg.Any<object>(), allowedNull).Returns(prompt);
            ILoopDataRetriever<int?> retriever = Fixture.Create<LoopDataRetriever<int?>>();

            retriever.ReadPrimitive(mockValidator, mockOutput, Fixture.Create<string>());

            mockOutput.WriteLineWithEscape(errorMessage);
        }

        [TestCase("1", false, (long)1)]
        [TestCase("1", true, (long)1)]
        [TestCase("", true, null)]
        public void ReadPrimitiveLong_MissingValidator_ReturnsReadedValue(string readedText, bool allowedNull, long? expected)
        {
            var mockOutput = Fixture.FreezeMock<IOutput>();
            var mockPrompt = Fixture.FreezeMock<IPrompt<string>>();
            var mockPromptFactory = Fixture.FreezeMock<IPromptFactory>();
            mockPrompt.Show(Arg.Any<IAnsiConsole>()).Returns(readedText);
            mockPromptFactory.CreateTextPrompt(Arg.Any<string>(), Arg.Any<object>(), allowedNull).Returns(mockPrompt);
            ILoopDataRetriever<long?> retriever = Fixture.Create<LoopDataRetriever<long?>>();

            long? actual = retriever.ReadPrimitive(null, mockOutput, Fixture.Create<string>());

            actual.Should().Be(expected);
        }

        [TestCase("1", false, (long)1)]
        [TestCase("1", true, (long)1)]
        [TestCase("", true, null)]
        public void ReadPrimitiveLong_GiveValidatorPassData_ReturnsReadedValue(string readedText, bool allowedNull, long? expected)
        {
            var mockOutput = Fixture.FreezeMock<IOutput>();
            var mockValidator = Fixture.FreezeMock<IValidator<long?>>();
            var succesValidation = new FluentValidation.Results.ValidationResult();
            mockValidator.Validate(expected).Returns(succesValidation);
            var prompt = Fixture.FreezeMock<IPrompt<string>>();
            var promptFactory = Fixture.FreezeMock<IPromptFactory>();

            prompt.Show(Arg.Any<IAnsiConsole>()).Returns(readedText);
            promptFactory.CreateTextPrompt(Arg.Any<string>(), Arg.Any<object>(), allowedNull).Returns(prompt);
            ILoopDataRetriever<long?> retriever = Fixture.Create<LoopDataRetriever<long?>>();

            long? actual = retriever.ReadPrimitive(mockValidator, mockOutput, Fixture.Create<string>());

            actual.Should().Be(expected);
        }

        [TestCase("1", false, (long)1)]
        [TestCase("1", true, (long)1)]
        [TestCase("", true, null)]
        public void ReadPrimitiveLong_GiveValidatorNotPassData_ChecksPrintErrors(string readedText, bool allowedNull, long? expected)
        {
            var mockOutput = Fixture.FreezeMock<IOutput>();
            var mockValidator = Fixture.FreezeMock<IValidator<long?>>();
            var propertyNameError = Fixture.FreezeMock<string>();
            var errorMessage = Fixture.FreezeMock<string>();
            var faliedValidation = new FluentValidation.Results.ValidationResult();
            faliedValidation.Errors.Add(new ValidationFailure(propertyNameError, errorMessage));
            var succesValidation = new FluentValidation.Results.ValidationResult();
            mockValidator.Validate(expected).Returns(faliedValidation, succesValidation);
            var prompt = Fixture.FreezeMock<IPrompt<string>>();
            var promptFactory = Fixture.FreezeMock<IPromptFactory>();

            prompt.Show(Arg.Any<IAnsiConsole>()).Returns(readedText);
            promptFactory.CreateTextPrompt(Arg.Any<string>(), Arg.Any<object>(), allowedNull).Returns(prompt);
            ILoopDataRetriever<long?> retriever = Fixture.Create<LoopDataRetriever<long?>>();

            retriever.ReadPrimitive(mockValidator, mockOutput, Fixture.Create<string>());

            mockOutput.WriteLineWithEscape(errorMessage);
        }

        [TestCase("1", false, 1.0f)]
        [TestCase("1", true, 1.0f)]
        [TestCase("", true, null)]
        public void ReadPrimitiveFloat_MissingValidator_ReturnsReadedValue(string readedText, bool allowedNull, float? expected)
        {
            var mockOutput = Fixture.FreezeMock<IOutput>();
            var mockPrompt = Fixture.FreezeMock<IPrompt<string>>();
            var mockPromptFactory = Fixture.FreezeMock<IPromptFactory>();
            mockPrompt.Show(Arg.Any<IAnsiConsole>()).Returns(readedText);
            mockPromptFactory.CreateTextPrompt(Arg.Any<string>(), Arg.Any<object>(), allowedNull).Returns(mockPrompt);
            ILoopDataRetriever<float?> retriever = Fixture.Create<LoopDataRetriever<float?>>();

            float? actual = retriever.ReadPrimitive(null, mockOutput, Fixture.Create<string>());

            actual.Should().Be(expected);
        }

        [TestCase("1", false, 1.0f)]
        [TestCase("1", true, 1.0f)]
        [TestCase("", true, null)]
        public void ReadPrimitiveFloat_GiveValidatorPassData_ReturnsReadedValue(string readedText, bool allowedNull, float? expected)
        {
            var mockOutput = Fixture.FreezeMock<IOutput>();
            var mockValidator = Fixture.FreezeMock<IValidator<float?>>();
            var succesValidation = new FluentValidation.Results.ValidationResult();
            mockValidator.Validate(expected).Returns(succesValidation);
            var prompt = Fixture.FreezeMock<IPrompt<string>>();
            var promptFactory = Fixture.FreezeMock<IPromptFactory>();

            prompt.Show(Arg.Any<IAnsiConsole>()).Returns(readedText);
            promptFactory.CreateTextPrompt(Arg.Any<string>(), Arg.Any<object>(), allowedNull).Returns(prompt);
            ILoopDataRetriever<float?> retriever = Fixture.Create<LoopDataRetriever<float?>>();

            float? actual = retriever.ReadPrimitive(mockValidator, mockOutput, Fixture.Create<string>());

            actual.Should().Be(expected);
        }

        [TestCase("1", false, 1.0f)]
        [TestCase("1", true, 1.0f)]
        [TestCase("", true, null)]
        public void ReadPrimitiveFloat_GiveValidatorNotPassData_ChecksPrintErrors(string readedText, bool allowedNull, float? expected)
        {
            var mockOutput = Fixture.FreezeMock<IOutput>();
            var mockValidator = Fixture.FreezeMock<IValidator<float?>>();
            var propertyNameError = Fixture.FreezeMock<string>();
            var errorMessage = Fixture.FreezeMock<string>();
            var faliedValidation = new FluentValidation.Results.ValidationResult();
            faliedValidation.Errors.Add(new ValidationFailure(propertyNameError, errorMessage));
            var succesValidation = new FluentValidation.Results.ValidationResult();
            mockValidator.Validate(expected).Returns(faliedValidation, succesValidation);
            var prompt = Fixture.FreezeMock<IPrompt<string>>();
            var promptFactory = Fixture.FreezeMock<IPromptFactory>();

            prompt.Show(Arg.Any<IAnsiConsole>()).Returns(readedText);
            promptFactory.CreateTextPrompt(Arg.Any<string>(), Arg.Any<object>(), allowedNull).Returns(prompt);
            ILoopDataRetriever<float?> retriever = Fixture.Create<LoopDataRetriever<float?>>();

            retriever.ReadPrimitive(mockValidator, mockOutput, Fixture.Create<string>());

            mockOutput.WriteLineWithEscape(errorMessage);
        }

        [TestCase("1", false, 1.0d)]
        [TestCase("1", true, 1.0d)]
        [TestCase("", true, null)]
        public void ReadPrimitiveDouble_MissingValidator_ReturnsReadedValue(string readedText, bool allowedNull, double? expected)
        {
            var mockOutput = Fixture.FreezeMock<IOutput>();
            var mockPrompt = Fixture.FreezeMock<IPrompt<string>>();
            var mockPromptFactory = Fixture.FreezeMock<IPromptFactory>();
            mockPrompt.Show(Arg.Any<IAnsiConsole>()).Returns(readedText);
            mockPromptFactory.CreateTextPrompt(Arg.Any<string>(), Arg.Any<object>(), allowedNull).Returns(mockPrompt);
            ILoopDataRetriever<double?> retriever = Fixture.Create<LoopDataRetriever<double?>>();

            double? actual = retriever.ReadPrimitive(null, mockOutput, Fixture.Create<string>());

            actual.Should().Be(expected);
        }

        [TestCase("1", false, 1.0d)]
        [TestCase("1", true, 1.0d)]
        [TestCase("", true, null)]
        public void ReadPrimitiveDouble_GiveValidatorPassData_ReturnsReadedValue(string readedText, bool allowedNull, double? expected)
        {
            var mockOutput = Fixture.FreezeMock<IOutput>();
            var mockValidator = Fixture.FreezeMock<IValidator<double?>>();
            var succesValidation = new FluentValidation.Results.ValidationResult();
            mockValidator.Validate(expected).Returns(succesValidation);
            var prompt = Fixture.FreezeMock<IPrompt<string>>();
            var promptFactory = Fixture.FreezeMock<IPromptFactory>();

            prompt.Show(Arg.Any<IAnsiConsole>()).Returns(readedText);
            promptFactory.CreateTextPrompt(Arg.Any<string>(), Arg.Any<object>(), allowedNull).Returns(prompt);
            ILoopDataRetriever<double?> retriever = Fixture.Create<LoopDataRetriever<double?>>();

            double? actual = retriever.ReadPrimitive(mockValidator, mockOutput, Fixture.Create<string>());

            actual.Should().Be(expected);
        }

        [TestCase("1", false, 1.0d)]
        [TestCase("1", true, 1.0d)]
        [TestCase("", true, null)]
        public void ReadPrimitiveDouble_GiveValidatorNotPassData_ChecksPrintErrors(string readedText, bool allowedNull, double? expected)
        {
            var mockOutput = Fixture.FreezeMock<IOutput>();
            var mockValidator = Fixture.FreezeMock<IValidator<double?>>();
            var propertyNameError = Fixture.FreezeMock<string>();
            var errorMessage = Fixture.FreezeMock<string>();
            var faliedValidation = new FluentValidation.Results.ValidationResult();
            faliedValidation.Errors.Add(new ValidationFailure(propertyNameError, errorMessage));
            var succesValidation = new FluentValidation.Results.ValidationResult();
            mockValidator.Validate(expected).Returns(faliedValidation, succesValidation);
            var prompt = Fixture.FreezeMock<IPrompt<string>>();
            var promptFactory = Fixture.FreezeMock<IPromptFactory>();

            prompt.Show(Arg.Any<IAnsiConsole>()).Returns(readedText);
            promptFactory.CreateTextPrompt(Arg.Any<string>(), Arg.Any<object>(), allowedNull).Returns(prompt);
            ILoopDataRetriever<double?> retriever = Fixture.Create<LoopDataRetriever<double?>>();

            retriever.ReadPrimitive(mockValidator, mockOutput, Fixture.Create<string>());

            mockOutput.WriteLineWithEscape(errorMessage);
        }

        [TestCase("1", false, 1.0)]
        [TestCase("1", true, 1.0)]
        [TestCase("", true, null)]
        public void ReadPrimitiveDecimal_MissingValidator_ReturnsReadedValue(string readedText, bool allowedNull, decimal? expected)
        {
            var mockOutput = Fixture.FreezeMock<IOutput>();
            var mockPrompt = Fixture.FreezeMock<IPrompt<string>>();
            var mockPromptFactory = Fixture.FreezeMock<IPromptFactory>();
            mockPrompt.Show(Arg.Any<IAnsiConsole>()).Returns(readedText);
            mockPromptFactory.CreateTextPrompt(Arg.Any<string>(), Arg.Any<object>(), allowedNull).Returns(mockPrompt);
            ILoopDataRetriever<decimal?> retriever = Fixture.Create<LoopDataRetriever<decimal?>>();

            decimal? actual = retriever.ReadPrimitive(null, mockOutput, Fixture.Create<string>());

            actual.Should().Be(expected);
        }

        [TestCase("1", false, 1.0)]
        [TestCase("1", true, 1.0)]
        [TestCase("", true, null)]
        public void ReadPrimitiveDecimal_GiveValidatorPassData_ReturnsReadedValue(string readedText, bool allowedNull, decimal? expected)
        {
            var mockOutput = Fixture.FreezeMock<IOutput>();
            var mockValidator = Fixture.FreezeMock<IValidator<decimal?>>();
            var succesValidation = new FluentValidation.Results.ValidationResult();
            mockValidator.Validate(expected).Returns(succesValidation);
            var prompt = Fixture.FreezeMock<IPrompt<string>>();
            var promptFactory = Fixture.FreezeMock<IPromptFactory>();

            prompt.Show(Arg.Any<IAnsiConsole>()).Returns(readedText);
            promptFactory.CreateTextPrompt(Arg.Any<string>(), Arg.Any<object>(), allowedNull).Returns(prompt);
            ILoopDataRetriever<decimal?> retriever = Fixture.Create<LoopDataRetriever<decimal?>>();

            decimal? actual = retriever.ReadPrimitive(mockValidator, mockOutput, Fixture.Create<string>());

            actual.Should().Be(expected);
        }

        [TestCase("1", false, 1.0)]
        [TestCase("1", true, 1.0)]
        [TestCase("", true, null)]
        public void ReadPrimitiveDecimal_GiveValidatorNotPassData_ChecksPrintErrors(string readedText, bool allowedNull, decimal? expected)
        {
            var mockOutput = Fixture.FreezeMock<IOutput>();
            var mockValidator = Fixture.FreezeMock<IValidator<decimal?>>();
            var propertyNameError = Fixture.FreezeMock<string>();
            var errorMessage = Fixture.FreezeMock<string>();
            var faliedValidation = new FluentValidation.Results.ValidationResult();
            faliedValidation.Errors.Add(new ValidationFailure(propertyNameError, errorMessage));
            var succesValidation = new FluentValidation.Results.ValidationResult();
            mockValidator.Validate(expected).Returns(faliedValidation, succesValidation);
            var prompt = Fixture.FreezeMock<IPrompt<string>>();
            var promptFactory = Fixture.FreezeMock<IPromptFactory>();

            prompt.Show(Arg.Any<IAnsiConsole>()).Returns(readedText);
            promptFactory.CreateTextPrompt(Arg.Any<string>(), Arg.Any<object>(), allowedNull).Returns(prompt);
            ILoopDataRetriever<decimal?> retriever = Fixture.Create<LoopDataRetriever<decimal?>>();

            retriever.ReadPrimitive(mockValidator, mockOutput, Fixture.Create<string>());

            mockOutput.WriteLineWithEscape(errorMessage);
        }

        [Test]
        public void ReadObject_GiveNotPrimitive_ThrowsInvalidOperationException()
        {
            var mockOutput = Fixture.FreezeMock<IOutput>();
            Action act = () =>
            {
                ILoopDataRetriever<int> retriever = Fixture.Create<LoopDataRetriever<int>>();
                retriever.ReadObject(null, mockOutput);
            };

            act.Should()
               .Throw<InvalidOperationException>()
               .WithMessage("Type is primitive");
        }

        [TestCase(false, "true", "None", "1", "25-12-1990", "1", "1", "1", "1", "00000000-0000-0000-0000-000000000001", true, TestEnum.None, 1, "foo", (long)1, 1f, 1d, 1)]
        [TestCase(true, "true", "None", "1", "25-12-1990", "1", "1", "1", "1", "00000000-0000-0000-0000-000000000001", true, TestEnum.None, 1, "foo", (long)1, 1f, 1d, 1)]
        [TestCase(true, "", "", "", "", "", "", "", "", "", null, null, null, "", null, null, null, null)]
        public void ReadObject_MissingValidator_ReturnsObject(bool allowedNull,
            string textBool,
            string textEnum,
            string textInt,
            string textDateTime,
            string textLong,
            string textFloat,
            string textDecimal,
            string textDouble,
            string textGuid,
            bool? expectedBool,
            TestEnum? expectedEnum,
            int? expectedInt,
            string? expectedString,
            long? expectedLong,
            float? expectedFloat,
            decimal? expectedDecimal,
            double? expectedDouble)
        {
            Guid? expectedGuid = string.IsNullOrEmpty(textGuid) ? null : Guid.Parse(textGuid);
            DateTime? expectedDatetime = string.IsNullOrEmpty(textDateTime) ? null : DateTime.Parse(textDateTime);
            var mockOutput = Fixture.FreezeMock<IOutput>();
            var mockPrompt = Fixture.FreezeMock<IPrompt<string>>();
            var mockPromptFactory = Fixture.FreezeMock<IPromptFactory>();
            mockPrompt.Show(Arg.Any<IAnsiConsole>()).Returns(textBool, textEnum, textInt, textLong, textDecimal, textDouble, textFloat, textGuid, expectedString, textDateTime);
            mockPromptFactory.CreateTextPrompt(Arg.Any<string>(), Arg.Any<object>(), allowedNull).Returns(mockPrompt);
            mockPromptFactory.CreateSelectionPrompt(Arg.Any<string>(), Arg.Any<object>(), allowedNull).Returns(mockPrompt);
            ILoopDataRetriever<TestObject?> retriever = Fixture.Create<LoopDataRetriever<TestObject?>>();

            TestObject? actual = retriever.ReadObject(null, mockOutput);

            actual?.BoolField.Should().Be(expectedBool);
            actual?.EnumField.Should().Be(expectedEnum);
            actual?.StringField.Should().Be(expectedString);
            actual?.IntField.Should().Be(expectedInt);
            actual?.DateTimeField.Should().Be(expectedDatetime);
            actual?.LongField.Should().Be(expectedLong);
            actual?.FloatField.Should().Be(expectedFloat);
            actual?.DecimalField.Should().Be(expectedDecimal);
            actual?.DoubleField.Should().Be(expectedDouble);
            actual?.GuidField.Should().Be(expectedGuid);
        }

        [TestCase(false, "true", "None", "1", "foo", "25-12-1990", "1", "1", "1", "1", "00000000-0000-0000-0000-000000000001", true, TestEnum.None, 1, "foo", (long)1, 1f, 1d, 1)]
        [TestCase(true, "true", "None", "1", "", "25-12-1990", "1", "1", "1", "1", "00000000-0000-0000-0000-000000000001", true, TestEnum.None, 1, "", (long)1, 1f, 1d, 1)]
        [TestCase(true, "", "", "", "", "", "", "", "", "", "", null, null, null, "", null, null, null, null)]
        public void ReadObject_GiveValidatorPassData_ReturnsObject(bool allowedNull,
            string textBool,
            string textEnum,
            string textInt,
            string textString,
            string textDateTime,
            string textLong,
            string textFloat,
            string textDecimal,
            string textDouble,
            string textGuid,
            bool? expectedBool,
            TestEnum? expectedEnum,
            int? expectedInt,
            string? expectedString,
            long? expectedLong,
            float? expectedFloat,
            decimal? expectedDecimal,
            double? expectedDouble)
        {
            Guid? expectedGuid = string.IsNullOrEmpty(textGuid) ? null : Guid.Parse(textGuid);
            DateTime? expectedDatetime = string.IsNullOrEmpty(textDateTime) ? null : DateTime.Parse(textDateTime);
            var mockOutput = Fixture.FreezeMock<IOutput>();
            var mockValidator = Fixture.FreezeMock<IValidator<TestObject?>>();
            var succesValidation = new FluentValidation.Results.ValidationResult();
            mockValidator.Validate(Arg.Any<TestObject?>()).Returns(succesValidation);
            var mockPrompt = Fixture.FreezeMock<IPrompt<string>>();
            var mockPromptFactory = Fixture.FreezeMock<IPromptFactory>();

            mockPrompt.Show(Arg.Any<IAnsiConsole>()).Returns(textBool, textEnum, textInt, textLong, textDecimal, textDouble, textFloat, textGuid, textString, textDateTime);
            mockPromptFactory.CreateTextPrompt(Arg.Any<string>(), Arg.Any<object>(), allowedNull).Returns(mockPrompt);
            mockPromptFactory.CreateSelectionPrompt(Arg.Any<string>(), Arg.Any<object>(), allowedNull).Returns(mockPrompt);
            ILoopDataRetriever<TestObject?> retriever = Fixture.Create<LoopDataRetriever<TestObject?>>();

            TestObject? actual = retriever.ReadObject(mockValidator, mockOutput);

            actual?.BoolField.Should().Be(expectedBool);
            actual?.EnumField.Should().Be(expectedEnum);
            actual?.StringField.Should().Be(expectedString);
            actual?.IntField.Should().Be(expectedInt);
            actual?.DateTimeField.Should().Be(expectedDatetime);
            actual?.LongField.Should().Be(expectedLong);
            actual?.FloatField.Should().Be(expectedFloat);
            actual?.DecimalField.Should().Be(expectedDecimal);
            actual?.DoubleField.Should().Be(expectedDouble);
            actual?.GuidField.Should().Be(expectedGuid);
        }

        [TestCase(false, "true", "None", "1", "foo", "25-12-1990", "1", "1", "1", "1", "00000000-0000-0000-0000-000000000001", true, TestEnum.None, 1, "foo", (long)1, 1f, 1d, 1)]
        [TestCase(true, "true", "None", "1", "", "25-12-1990", "1", "1", "1", "1", "00000000-0000-0000-0000-000000000001", true, TestEnum.None, 1, "", (long)1, 1f, 1d, 1)]
        [TestCase(true, "", "", "", "", "", "", "", "", "", "", null, null, null, "", null, null, null, null)]
        public void ReadObject_GiveValidatorNotPassData_ChecksPrintErrors(bool allowedNull,
            string textBool,
            string textEnum,
            string textInt,
            string textString,
            string textDateTime,
            string textLong,
            string textFloat,
            string textDecimal,
            string textDouble,
            string textGuid,
            bool? expectedBool,
            TestEnum? expectedEnum,
            int? expectedInt,
            string? expectedString,
            long? expectedLong,
            float? expectedFloat,
            decimal? expectedDecimal,
            double? expectedDouble)
        {
            Guid? expectedGuid = string.IsNullOrEmpty(textGuid) ? null : Guid.Parse(textGuid);
            DateTime? expectedDatetime = string.IsNullOrEmpty(textDateTime) ? null : DateTime.Parse(textDateTime);
            var mockOutput = Fixture.FreezeMock<IOutput>();
            var mockValidator = Fixture.FreezeMock<IValidator<TestObject?>>();
            var propertyNameError = Fixture.FreezeMock<string>();
            var errorMessage = Fixture.FreezeMock<string>();
            var faliedValidation = new FluentValidation.Results.ValidationResult();
            faliedValidation.Errors.Add(new ValidationFailure(propertyNameError, errorMessage));
            var succesValidation = new FluentValidation.Results.ValidationResult();
            mockValidator.Validate(Arg.Any<TestObject?>()).Returns(faliedValidation, succesValidation);
            var mockPrompt = Fixture.FreezeMock<IPrompt<string>>();
            var mockPromptFactory = Fixture.FreezeMock<IPromptFactory>();

            mockPrompt.Show(Arg.Any<IAnsiConsole>()).Returns(textBool, textEnum, textInt, textLong, textDecimal, textDouble, textFloat, textGuid, textString, textDateTime);
            mockPromptFactory.CreateTextPrompt(Arg.Any<string>(), Arg.Any<object>(), allowedNull).Returns(mockPrompt);
            mockPromptFactory.CreateSelectionPrompt(Arg.Any<string>(), Arg.Any<object>(), allowedNull).Returns(mockPrompt);
            ILoopDataRetriever<TestObject?> retriever = Fixture.Create<LoopDataRetriever<TestObject?>>();

            retriever.ReadObject(mockValidator, mockOutput);

            mockOutput.WriteLineWithEscape(errorMessage);
        }

        [Test]
        public void ReadObject_GiveNotPrimitive_ThrowsNotImplementedException()
        {
            var mockOutput = Fixture.FreezeMock<IOutput>();
            Action act = () =>
            {
                ILoopDataRetriever<NeastedTestObject> retriever = Fixture.Create<LoopDataRetriever<NeastedTestObject>>();
                retriever.ReadObject(null, mockOutput);
            };

            act.Should()
               .Throw<NotImplementedException>()
               .WithMessage("Not defined type for read");
        }
    }
}
