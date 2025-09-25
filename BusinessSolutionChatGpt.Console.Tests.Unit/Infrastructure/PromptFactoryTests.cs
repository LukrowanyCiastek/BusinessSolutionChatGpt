using AutoFixture;
using BusinessSolutionChatGpt.Console.Infrastructure;
using BusinessSolutionChatGpt.Console.Infrastructure.Interfaces;
using BusinessSolutionChatGpt.Tests.Core;
using FluentAssertions;
using NSubstitute;
using Spectre.Console;

namespace BusinessSolutionChatGpt.Console.Tests.Unit.Infrastructure
{
    [TestFixture]
    public class PromptFactoryTests : BaseFixture
    {
        [TestCase(true)]
        [TestCase(false)]
        public void CreateTextPrompt_Called_LabelFactoryWasUsedReturnsTextPrompt(bool isNullable)
        {
            var label = Fixture.Create<string>();
            var current = Fixture.Create<object>();
            var promptLabelFactoryMock = Fixture.FreezeMock<IPromptLabelFactory>();
            IPromptFactory factory = Fixture.Create<PromptFactory>();

            var actual = factory.CreateTextPrompt(label, current, isNullable);

            promptLabelFactoryMock.Received().Create(label, current, isNullable);
            actual.Should().BeOfType<TextPrompt<string>>();
        }

        [TestCase(true)]
        [TestCase(false)]
        public void CreateSelectionPrompt_Called_LabelFactoryWasUsedReturnsTextPrompt(bool isNullable)
        {
            var label = Fixture.Create<string>();
            var current = Fixture.Create<object>();
            var promptLabelFactoryMock = Fixture.FreezeMock<IPromptLabelFactory>();
            IPromptFactory factory = Fixture.Create<PromptFactory>();

            var actual = factory.CreateSelectionPrompt(label, current, isNullable);

            promptLabelFactoryMock.Received().Create(label, current, isNullable);
            actual.Should().BeOfType<SelectionPrompt<string>>();
        }

        [TestCase(true, null, new string[] { "choice" })]
        [TestCase(false, null, new string[] {})]
        [TestCase(true, 3, new string[] { "choice" })]
        [TestCase(false, 3, new string[] { })]
        public void CreateSelectionPrompt_CalledWithPageSizeAndChoices_LabelFactoryWasUsedReturnsTextPrompt(bool isNullable, int? pageSize, string[] choices)
        {
            var label = Fixture.Create<string>();
            var current = Fixture.Create<object>();
            var promptLabelFactoryMock = Fixture.FreezeMock<IPromptLabelFactory>();
            IPromptFactory factory = Fixture.Create<PromptFactory>();

            var actual = factory.CreateSelectionPrompt(label, current, isNullable, pageSize, choices);

            promptLabelFactoryMock.Received().Create(label, current, isNullable);
            actual.Should().BeOfType<SelectionPrompt<string>>();
        }

        [Test]
        public void CreateSelectionPrompt_ChoicesIsNull_LabelFactoryWasUsedThrowsArgumentNullExceptions()
        {
            var label = Fixture.Create<string>();
            var current = Fixture.Create<object>();
            var isNullable = Fixture.Create<bool>();
            var promptLabelFactoryMock = Fixture.FreezeMock<IPromptLabelFactory>();
            IPromptFactory factory = Fixture.Create<PromptFactory>();

            Action act = () => factory.CreateSelectionPrompt(label, current, isNullable, Fixture.Create<int>(), null!);

            act.Should().Throw<ArgumentNullException>();
            promptLabelFactoryMock.Received().Create(label, current, isNullable);            
        }
    }
}
