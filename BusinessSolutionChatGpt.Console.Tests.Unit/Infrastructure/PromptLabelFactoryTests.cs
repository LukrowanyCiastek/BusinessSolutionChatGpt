using AutoFixture;
using BusinessSolutionChatGpt.Console.Infrastructure;
using BusinessSolutionChatGpt.Tests.Core;
using FluentAssertions;
using System.Globalization;

namespace BusinessSolutionChatGpt.Console.Tests.Unit.Infrastructure
{
    [TestFixture]
    public class PromptLabelFactoryTests : BaseFixture
    {
        [Test]
        public void Create_IsNullable_RetunrsFormattedTextForNullable()
        {
            var label = Fixture.Create<string>();
            var expected = $"[bold]{label}[/] [dim](Enter = puste)[/]";            
            var factory = Fixture.Create<PromptLabelFactory>();

            var actual = factory.Create(label, null, true);

            expected.Should().Be(actual);
        }

        [Test]
        public void Create_IsNotNullableObjectIsNull_RetunrsTextWithoutPartOfLabel()
        {
            var label = Fixture.Create<string>();
            var expected = $"[bold]{label}[/]";
            var factory = Fixture.Create<PromptLabelFactory>();

            var actual = factory.Create(label, null, false);

            expected.Should().Be(actual);
        }

        [Test]
        public void Create_IsNotNullableObjecIsNotNullAndIsDateTime_RetunrsTextWithDefaultValue()
        {
            var label = Fixture.Create<string>();
            var date = Fixture.Create<DateTime>();

            var expected = $"[bold]{label}[/] [dim](domyślnie: {date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)})[/]";
            var factory = Fixture.Create<PromptLabelFactory>();

            var actual = factory.Create(label, date, false);

            expected.Should().Be(actual);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Create_IsNotNullableObjecIsNotNullAndIsBool_RetunrsTextWithDefaultValue(bool condition)
        {
            var label = Fixture.Create<string>();
            var text = condition ? "tak" : "nie";
            var expected = $"[bold]{label}[/] [dim](domyślnie: {text})[/]";
            var factory = Fixture.Create<PromptLabelFactory>();

            var actual = factory.Create(label, condition, false);

            expected.Should().Be(actual);
        }

        [Test]
        public void Create_IsNotNullableObjecIsNotNullAndIsString_RetunrsTextWithDefaultValue()
        {
            var label = Fixture.Create<string>();
            var text = Fixture.Create<string>();

            var expected = $"[bold]{label}[/] [dim](domyślnie: {text})[/]";
            var factory = Fixture.Create<PromptLabelFactory>();

            var actual = factory.Create(label, text, false);

            expected.Should().Be(actual);
        }
    }
}
