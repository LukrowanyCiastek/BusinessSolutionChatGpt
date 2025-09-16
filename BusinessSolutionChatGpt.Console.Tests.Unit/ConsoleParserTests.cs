using AutoFixture;
using BusinessSolutionChatGpt.Console.Tests.Unit.Utils;
using BusinessSolutionChatGpt.Tests.Core;
using FluentAssertions;
using System.Globalization;

namespace BusinessSolutionChatGpt.Console.Tests.Unit
{
    [TestFixture]
    public class ConsoleParserTests : BaseFixture
    {
        private const string DefaultShortPlYes = "t";
        private const string DefaultPlYes = "tak";
        private const string DefaultShortEnYes = "y";
        private const string DefaultEnYes = "yes";
        private const string DefaultBoolTrue = "true";
        private const string DefaultOne = "1";
        private const string DefaultShortNo = "n";
        private const string DefaultPlNo = "nie";
        private const string DefaultEnNo = "no";
        private const string DefaultBoolFalse = "false";
        private const string DefaultZero = "0";
        private const string DefaultNumberWithColon = "1.25";
        private const string DefaultText = "Hello world";
        private const string DefaultEnDate = "1990-12-25";
        private const string DefaultPLDate = "25-12-1990";
        private const string NotExistedEnumNumber = "2";

        [TestCase(DefaultShortPlYes, true)]
        [TestCase(DefaultPlYes, true)]
        [TestCase(DefaultShortEnYes, true)]
        [TestCase(DefaultEnYes, true)]
        [TestCase(DefaultBoolTrue, true)]
        [TestCase(DefaultOne, true)]
        [TestCase(DefaultShortNo, false)]
        [TestCase(DefaultPlNo, false)]
        [TestCase(DefaultEnNo, false)]
        [TestCase(DefaultBoolFalse, false)]
        [TestCase(DefaultZero, false)]
        [TestCase(DefaultNumberWithColon, null)]
        [TestCase(DefaultText, null)]
        [TestCase(DefaultEnDate, null)]
        public void TryParseBool_GivenCorrectInput_OutMatchedBoolValue(string input, bool? expected)
        {
            ConsoleParser.TryParseBool(input, out object? actual);
            ((bool?)actual!).Should().Be(expected);
        }

        [TestCase(DefaultNumberWithColon)]
        [TestCase(DefaultText)]
        [TestCase(DefaultEnDate)]
        public void TryParseBool_GivenIncorrectInput_OutMatchedBoolValue(string input)
        {
            ConsoleParser.TryParseBool(input, out object? actual);
            actual.Should().BeNull();
        }

        [TestCase(DefaultShortPlYes)]
        [TestCase(DefaultPlYes)]
        [TestCase(DefaultShortEnYes)]
        [TestCase(DefaultEnYes)]
        [TestCase(DefaultBoolTrue)]
        [TestCase(DefaultOne)]
        [TestCase(DefaultShortNo)]
        [TestCase(DefaultPlNo)]
        [TestCase(DefaultEnNo)]
        [TestCase(DefaultBoolFalse)]
        [TestCase(DefaultZero)]
        public void TryParseBool_GivenCorrectInput_ReturnsMatchedBoolValue(string input)
        {
            var actual = ConsoleParser.TryParseBool(input, out object? result);
            actual.Should().BeTrue();
        }

        [Test]
        public void TryParseBool_GivenIncorrectInput_ReturnMatchedBoolValue()
        {
            var actual = ConsoleParser.TryParseBool(Fixture.Create<string>(), out object? result);
            actual.Should().BeFalse();
        }

        [Test]
        public void TryParseString_GiveString_OutGivenString()
        {
            var expected = Fixture.Create<string>();

            ConsoleParser.TryParseString(expected, out object? actual);

            ((string)actual!).Should().Be(expected);
        }

        [Test]
        public void TryParseString_GiveString_ReturnGivenString()
        {
            var expected = Fixture.Create<string>();

            var actual = ConsoleParser.TryParseString(expected, out object? result);

            actual.Should().BeTrue();
        }

        [TestCase(DefaultOne, 1)]
        [TestCase(DefaultNumberWithColon, null)]
        [TestCase(DefaultText, null)]
        [TestCase(DefaultEnDate, null)]
        public void TryParseInt_GiveInput_OutMachedValaue(string input, int? expected)
        {
            ConsoleParser.TryParseInt(input, out object? actual);

            actual.Should().Be(expected);
        }

        [TestCase(DefaultOne, true)]
        [TestCase(DefaultNumberWithColon, false)]
        [TestCase(DefaultText, false)]
        [TestCase(DefaultEnDate, false)]
        public void TryParseInt_GiveInput_ReturnValaue(string input, bool expected)
        {
            var actual = ConsoleParser.TryParseInt(input, out object? result);

            actual.Should().Be(expected);
        }

        [TestCase(DefaultOne, 1)]
        [TestCase(DefaultNumberWithColon, null)]
        [TestCase(DefaultText, null)]
        [TestCase(DefaultEnDate, null)]
        public void TryParseLong_GiveInput_OutMachedValaue(string input, long? expected)
        {
            ConsoleParser.TryParseLong(input, out object? actual);

            actual.Should().Be(expected);
        }

        [TestCase(DefaultOne, true)]
        [TestCase(DefaultNumberWithColon, false)]
        [TestCase(DefaultText, false)]
        [TestCase(DefaultEnDate, false)]
        public void TryParseLong_GiveInput_ReturnValaue(string input, bool expected)
        {
            var actual = ConsoleParser.TryParseLong(input, out object? result);

            actual.Should().Be(expected);
        }

        [TestCase(DefaultOne, 1)]
        [TestCase(DefaultNumberWithColon, 1.25)]
        [TestCase(DefaultText, null)]
        [TestCase(DefaultEnDate, null)]
        public void TryParseDecimal_GiveInput_OutMachedValaue(string input, decimal? expected)
        {
            ConsoleParser.TryParseDecimal(input, out object? actual);

            actual.Should().Be(expected);
        }

        [TestCase(DefaultOne, true)]
        [TestCase(DefaultNumberWithColon, true)]
        [TestCase(DefaultText, false)]
        [TestCase(DefaultEnDate, false)]
        public void TryParseDecimal_GiveInput_ReturnValaue(string input, bool expected)
        {
            var actual = ConsoleParser.TryParseDecimal(input, out object? result);

            actual.Should().Be(expected);
        }

        [TestCase(DefaultOne, 1)]
        [TestCase(DefaultNumberWithColon, 1.25)]
        [TestCase(DefaultText, null)]
        [TestCase(DefaultEnDate, null)]
        public void TryParseDouble_GiveInput_OutMachedValaue(string input, double? expected)
        {
            ConsoleParser.TryParseDouble(input, out object? actual);

            actual.Should().Be(expected);
        }

        [TestCase(DefaultOne, true)]
        [TestCase(DefaultNumberWithColon, true)]
        [TestCase(DefaultText, false)]
        [TestCase(DefaultEnDate, false)]
        public void TryParseDouble_GiveInput_ReturnValaue(string input, bool expected)
        {
            var actual = ConsoleParser.TryParseDouble(input, out object? result);

            actual.Should().Be(expected);
        }

        [Test]
        public void TryParseDateTimeExact_GiveInput_OutMachedValaue()
        {
            var input = DefaultEnDate;
            DateTime.TryParseExact(input, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var expected);
            ConsoleParser.TryParseDateTime(input, out object? actual);

            actual.Should().Be(expected);
        }

        [Test]
        public void TryParseDateTime_GiveInput_OutMachedValaue()
        {
            var input = DefaultPLDate;
            DateTime.TryParse(input, CultureInfo.CurrentCulture, DateTimeStyles.AssumeLocal, out var expected);
            ConsoleParser.TryParseDateTime(input, out object? actual);

            actual.Should().Be(expected);
        }

        [TestCase(DefaultOne, null)]
        [TestCase(DefaultNumberWithColon, null)]
        [TestCase(DefaultText, null)]
        public void TryParseDateTime_GiveInput_OutMachedValaue(string input, DateTime? expected)
        {
            ConsoleParser.TryParseDateTime(input, out object? actual);

            actual.Should().Be(expected);
        }

        [TestCase(DefaultEnDate)]
        [TestCase(DefaultPLDate)]
        public void TryParseDateTime_GiveInput_ReturnValaue(string input)
        {
            var actual = ConsoleParser.TryParseDateTime(input, out object? result);

            actual.Should().BeTrue();
        }

        [TestCase(DefaultOne, false)]
        [TestCase(DefaultNumberWithColon, false)]
        [TestCase(DefaultText, false)]
        public void TryParseDateTime_GiveInput_ReturnValaue(string input, bool expected)
        {
            var actual = ConsoleParser.TryParseDateTime(input, out object? result);

            actual.Should().Be(expected);
        }

        [TestCase(DefaultOne, TestEnum.None)]
        [TestCase(NotExistedEnumNumber, (TestEnum)2)]
        [TestCase(DefaultNumberWithColon, null)]
        [TestCase(DefaultText, null)]
        [TestCase(DefaultEnDate, null)]
        public void TryParseEnum_GiveInput_OutMachedValaue(string input, TestEnum? expected)
        {
            ConsoleParser.TryParseEnum(input, typeof(TestEnum), out object? actual);

            actual.Should().Be(expected);
        }

        [TestCase(DefaultOne, true)]
        [TestCase(NotExistedEnumNumber, true)]
        [TestCase(DefaultNumberWithColon, false)]
        [TestCase(DefaultText, false)]
        [TestCase(DefaultEnDate, false)]
        public void TryParseEnum_GiveInput_ReturnValaue(string input, bool expected)
        {
            var actual = ConsoleParser.TryParseEnum(input, typeof(TestEnum), out object? result);

            actual.Should().Be(expected);
        }

        [TestCase(DefaultShortPlYes, true)]
        [TestCase(DefaultPlYes, true)]
        [TestCase(DefaultShortEnYes, true)]
        [TestCase(DefaultEnYes, true)]
        [TestCase(DefaultBoolTrue, true)]
        [TestCase(DefaultOne, true)]
        [TestCase(DefaultShortNo, false)]
        [TestCase(DefaultPlNo, false)]
        [TestCase(DefaultEnNo, false)]
        [TestCase(DefaultBoolFalse, false)]
        [TestCase(DefaultZero, false)]
        [TestCase(DefaultNumberWithColon, null)]
        [TestCase(DefaultText, null)]
        [TestCase(DefaultEnDate, null)]
        [TestCase(" " + DefaultShortPlYes, true)]
        [TestCase(" " + DefaultPlYes, true)]
        [TestCase(" " + DefaultShortEnYes, true)]
        [TestCase(" " + DefaultEnYes, true)]
        [TestCase(" " + DefaultBoolTrue, true)]
        [TestCase(" " + DefaultOne, true)]
        [TestCase(" " + DefaultShortNo, false)]
        [TestCase(" " + DefaultPlNo, false)]
        [TestCase(" " + DefaultEnNo, false)]
        [TestCase(" " + DefaultBoolFalse, false)]
        [TestCase(" " + DefaultZero, false)]
        [TestCase(" " + DefaultNumberWithColon, null)]
        [TestCase(" " + DefaultText, null)]
        [TestCase(" " + DefaultEnDate, null)]
        [TestCase(DefaultShortPlYes + " ", true)]
        [TestCase(DefaultPlYes + " ", true)]
        [TestCase(DefaultShortEnYes + " ", true)]
        [TestCase(DefaultEnYes + " ", true)]
        [TestCase(DefaultBoolTrue + " ", true)]
        [TestCase(DefaultOne + " ", true)]
        [TestCase(DefaultShortNo + " ", false)]
        [TestCase(DefaultPlNo + " ", false)]
        [TestCase(DefaultEnNo + " ", false)]
        [TestCase(DefaultBoolFalse + " ", false)]
        [TestCase(DefaultZero + " ", false)]
        [TestCase(DefaultNumberWithColon + " ", null)]
        [TestCase(DefaultText + " ", null)]
        [TestCase(DefaultEnDate + " ", null)]
        public void TryParse_GivenCorrectInput_OutMatchedBoolValue(string input, bool? expected)
        {
            ConsoleParser.TryParse(input, typeof(bool), out object? actual);
            ((bool?)actual!).Should().Be(expected);
        }

        [TestCase(DefaultNumberWithColon)]
        [TestCase(DefaultText)]
        [TestCase(DefaultEnDate)]
        [TestCase(" " + DefaultNumberWithColon)]
        [TestCase(" " + DefaultText)]
        [TestCase(" " + DefaultEnDate)]
        [TestCase(DefaultNumberWithColon + " ")]
        [TestCase(DefaultText + " ")]
        [TestCase(DefaultEnDate + " ")]
        public void TryParse_GivenIncorrectInput_OutMatchedBoolValue(string input)
        {
            ConsoleParser.TryParse(input, typeof(bool), out object? actual);
            actual.Should().BeNull();
        }

        [TestCase(DefaultShortPlYes)]
        [TestCase(DefaultPlYes)]
        [TestCase(DefaultShortEnYes)]
        [TestCase(DefaultEnYes)]
        [TestCase(DefaultBoolTrue)]
        [TestCase(DefaultOne)]
        [TestCase(DefaultShortNo)]
        [TestCase(DefaultPlNo)]
        [TestCase(DefaultEnNo)]
        [TestCase(DefaultBoolFalse)]
        [TestCase(DefaultZero)]
        [TestCase(" " + DefaultShortPlYes)]
        [TestCase(" " + DefaultPlYes)]
        [TestCase(" " + DefaultShortEnYes)]
        [TestCase(" " + DefaultEnYes)]
        [TestCase(" " + DefaultBoolTrue)]
        [TestCase(" " + DefaultOne)]
        [TestCase(" " + DefaultShortNo)]
        [TestCase(" " + DefaultPlNo)]
        [TestCase(" " + DefaultEnNo)]
        [TestCase(" " + DefaultBoolFalse)]
        [TestCase(" " + DefaultZero)]
        [TestCase(DefaultShortPlYes + " ")]
        [TestCase(DefaultPlYes + " ")]
        [TestCase(DefaultShortEnYes + " ")]
        [TestCase(DefaultEnYes + " ")]
        [TestCase(DefaultBoolTrue + " ")]
        [TestCase(DefaultOne + " ")]
        [TestCase(DefaultShortNo + " ")]
        [TestCase(DefaultPlNo + " ")]
        [TestCase(DefaultEnNo + " ")]
        [TestCase(DefaultBoolFalse + " ")]
        [TestCase(DefaultZero + " ")]
        public void TryParse_GivenCorrectInput_ReturnsMatchedBoolValue(string input)
        {
            var actual = ConsoleParser.TryParse(input, typeof(bool), out object? result);
            actual.Should().BeTrue();
        }

        [Test]
        public void TryParse_GivenIncorrectInput_ReturnMatchedBoolValue()
        {
            var actual = ConsoleParser.TryParse(Fixture.Create<string>(), typeof(bool), out object? result);
            actual.Should().BeFalse();
        }

        [Test]
        public void TryParse_GiveString_OutGivenString()
        {
            var expected = Fixture.Create<string>();

            ConsoleParser.TryParse(expected, typeof(string), out object? actual);

            ((string)actual!).Should().Be(expected);
        }

        [Test]
        public void TryParse_GiveString_ReturnGivenString()
        {
            var expected = Fixture.Create<string>();

            var actual = ConsoleParser.TryParse(expected, typeof(string), out object? result);

            actual.Should().BeTrue();
        }

        [TestCase(DefaultOne, 1)]
        [TestCase(DefaultNumberWithColon, null)]
        [TestCase(DefaultText, null)]
        [TestCase(DefaultEnDate, null)]
        [TestCase(" " + DefaultOne, 1)]
        [TestCase(" " + DefaultNumberWithColon, null)]
        [TestCase(" " + DefaultText, null)]
        [TestCase(" " + DefaultEnDate, null)]
        [TestCase(DefaultOne + " ", 1)]
        [TestCase(DefaultNumberWithColon + " ", null)]
        [TestCase(DefaultText + " ", null)]
        [TestCase(DefaultEnDate + " ", null)]
        public void TryParse_GiveInput_OutMachedValaue(string input, int? expected)
        {
            ConsoleParser.TryParse(input, typeof(int), out object? actual);

            actual.Should().Be(expected);
        }

        [TestCase(DefaultOne, true)]
        [TestCase(DefaultNumberWithColon, false)]
        [TestCase(DefaultText, false)]
        [TestCase(DefaultEnDate, false)]
        [TestCase(" " + DefaultOne, true)]
        [TestCase(" " + DefaultNumberWithColon, false)]
        [TestCase(" " + DefaultText, false)]
        [TestCase(" " + DefaultEnDate, false)]
        [TestCase(DefaultOne + " ", true)]
        [TestCase(DefaultNumberWithColon + " ", false)]
        [TestCase(DefaultText + " ", false)]
        [TestCase(DefaultEnDate + " ", false)]
        public void TryParseForInt_GiveInput_ReturnValaue(string input, bool expected)
        {
            var actual = ConsoleParser.TryParse(input, typeof(int), out object? result);

            actual.Should().Be(expected);
        }

        [TestCase(DefaultOne, 1)]
        [TestCase(DefaultNumberWithColon, null)]
        [TestCase(DefaultText, null)]
        [TestCase(DefaultEnDate, null)]
        [TestCase(" " + DefaultOne, 1)]
        [TestCase(" " + DefaultNumberWithColon, null)]
        [TestCase(" " + DefaultText, null)]
        [TestCase(" " + DefaultEnDate, null)]
        [TestCase(DefaultOne + " ", 1)]
        [TestCase(DefaultNumberWithColon + " ", null)]
        [TestCase(DefaultText + " ", null)]
        [TestCase(DefaultEnDate + " ", null)]
        public void TryParse_GiveInput_OutMachedValaue(string input, long? expected)
        {
            ConsoleParser.TryParse(input, typeof(long), out object? actual);

            actual.Should().Be(expected);
        }

        [TestCase(DefaultOne, true)]
        [TestCase(DefaultNumberWithColon, false)]
        [TestCase(DefaultText, false)]
        [TestCase(DefaultEnDate, false)]
        [TestCase(" " + DefaultOne, true)]
        [TestCase(" " + DefaultNumberWithColon, false)]
        [TestCase(" " + DefaultText, false)]
        [TestCase(" " + DefaultEnDate, false)]
        [TestCase(DefaultOne + " ", true)]
        [TestCase(DefaultNumberWithColon + " ", false)]
        [TestCase(DefaultText + " ", false)]
        [TestCase(DefaultEnDate + " ", false)]
        public void TryParseForLong_GiveInput_ReturnValaue(string input, bool expected)
        {
            var actual = ConsoleParser.TryParse(input, typeof(long), out object? result);

            actual.Should().Be(expected);
        }

        [TestCase(DefaultOne, 1)]
        [TestCase(DefaultNumberWithColon, 1.25)]
        [TestCase(DefaultText, null)]
        [TestCase(DefaultEnDate, null)]
        [TestCase(" " + DefaultOne, 1)]
        [TestCase(" " + DefaultNumberWithColon, 1.25)]
        [TestCase(" " + DefaultText, null)]
        [TestCase(" " + DefaultEnDate, null)]
        [TestCase(DefaultOne + " ", 1)]
        [TestCase(DefaultNumberWithColon + " ", 1.25)]
        [TestCase(DefaultText + " ", null)]
        [TestCase(DefaultEnDate + " ", null)]

        public void TryParse_GiveInput_OutMachedValaue(string input, decimal? expected)
        {
            ConsoleParser.TryParse(input, typeof(decimal), out object? actual);

            actual.Should().Be(expected);
        }

        [TestCase(DefaultOne, true)]
        [TestCase(DefaultNumberWithColon, true)]
        [TestCase(DefaultText, false)]
        [TestCase(DefaultEnDate, false)]
        [TestCase(" " + DefaultOne, true)]
        [TestCase(" " + DefaultNumberWithColon, true)]
        [TestCase(" " + DefaultText, false)]
        [TestCase(" " + DefaultEnDate, false)]
        [TestCase(DefaultOne + " ", true)]
        [TestCase(DefaultNumberWithColon + " ", true)]
        [TestCase(DefaultText + " ", false)]
        [TestCase(DefaultEnDate + " ", false)]
        public void TryParseForDecimal_GiveInput_ReturnValaue(string input, bool expected)
        {
            var actual = ConsoleParser.TryParse(input, typeof(decimal), out object? result);

            actual.Should().Be(expected);
        }

        [TestCase(DefaultOne, 1)]
        [TestCase(DefaultNumberWithColon, 1.25)]
        [TestCase(DefaultText, null)]
        [TestCase(DefaultEnDate, null)]
        [TestCase(" " + DefaultOne, 1)]
        [TestCase(" " + DefaultNumberWithColon, 1.25)]
        [TestCase(" " + DefaultText, null)]
        [TestCase(" " + DefaultEnDate, null)]
        [TestCase(DefaultOne + " ", 1)]
        [TestCase(DefaultNumberWithColon + " ", 1.25)]
        [TestCase(DefaultText + " ", null)]
        [TestCase(DefaultEnDate + " ", null)]
        public void TryParse_GiveInput_OutMachedValaue(string input, double? expected)
        {
            ConsoleParser.TryParse(input, typeof(double), out object? actual);

            actual.Should().Be(expected);
        }

        [TestCase(DefaultOne, true)]
        [TestCase(DefaultNumberWithColon, true)]
        [TestCase(DefaultText, false)]
        [TestCase(DefaultEnDate, false)]
        [TestCase(" " + DefaultOne, true)]
        [TestCase(" " + DefaultNumberWithColon, true)]
        [TestCase(" " + DefaultText, false)]
        [TestCase(" " + DefaultEnDate, false)]
        [TestCase(DefaultOne + " ", true)]
        [TestCase(DefaultNumberWithColon + " ", true)]
        [TestCase(DefaultText + " ", false)]
        [TestCase(DefaultEnDate + " ", false)]
        public void TryParseForDouble_GiveInput_ReturnValaue(string input, bool expected)
        {
            var actual = ConsoleParser.TryParse(input, typeof(double), out object? result);

            actual.Should().Be(expected);
        }

        [Test]
        public void TryParse_GiveInput_OutMachedValaue()
        {
            var input = DefaultEnDate;
            DateTime.TryParseExact(input, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var expected);
            ConsoleParser.TryParse(input, typeof(DateTime), out object? actual);

            actual.Should().Be(expected);
        }

        [Test]
        public void TryParseForDateTime_GiveInput_OutMachedValaue()
        {
            var input = DefaultPLDate;
            DateTime.TryParse(input, CultureInfo.CurrentCulture, DateTimeStyles.AssumeLocal, out var expected);
            ConsoleParser.TryParse(input, typeof(DateTime), out object? actual);

            actual.Should().Be(expected);
        }

        [TestCase(DefaultOne, null)]
        [TestCase(DefaultNumberWithColon, null)]
        [TestCase(DefaultText, null)]
        [TestCase(" " + DefaultOne, null)]
        [TestCase(" " + DefaultNumberWithColon, null)]
        [TestCase(" " + DefaultText, null)]
        [TestCase(DefaultOne + " ", null)]
        [TestCase(DefaultNumberWithColon + " ", null)]
        [TestCase(DefaultText + " ", null)]
        public void TryParse_GiveInput_OutMachedValaue(string input, DateTime? expected)
        {
            ConsoleParser.TryParse(input, typeof(DateTime), out object? actual);

            actual.Should().Be(expected);
        }

        [TestCase(DefaultEnDate)]
        [TestCase(DefaultPLDate)]
        [TestCase(" " + DefaultEnDate)]
        [TestCase(" " + DefaultPLDate)]
        [TestCase(DefaultEnDate + " ")]
        [TestCase(DefaultPLDate + " ")]
        public void TryParse_GiveInput_ReturnValaue(string input)
        {
            var actual = ConsoleParser.TryParse(input, typeof(DateTime), out object? result);

            actual.Should().BeTrue();
        }

        [TestCase(DefaultOne, false)]
        [TestCase(DefaultNumberWithColon, false)]
        [TestCase(DefaultText, false)]
        [TestCase(" " + DefaultOne, false)]
        [TestCase(" " + DefaultNumberWithColon, false)]
        [TestCase(" " + DefaultText, false)]
        [TestCase(DefaultOne + " ", false)]
        [TestCase(DefaultNumberWithColon + " ", false)]
        [TestCase(DefaultText + " ", false)]
        public void TryParse_GiveInput_ReturnValaue(string input, bool expected)
        {
            var actual = ConsoleParser.TryParse(input, typeof(DateTime), out object? result);

            actual.Should().Be(expected);
        }

        [TestCase(DefaultOne, TestEnum.None)]
        [TestCase(NotExistedEnumNumber, (TestEnum)2)]
        [TestCase(DefaultNumberWithColon, null)]
        [TestCase(DefaultText, null)]
        [TestCase(DefaultEnDate, null)]
        [TestCase(" " + DefaultOne, TestEnum.None)]
        [TestCase(" " + NotExistedEnumNumber, (TestEnum)2)]
        [TestCase(" " + DefaultNumberWithColon, null)]
        [TestCase(" " + DefaultText, null)]
        [TestCase(" " + DefaultEnDate, null)]
        [TestCase(DefaultOne + " ", TestEnum.None)]
        [TestCase(NotExistedEnumNumber + " ", (TestEnum)2)]
        [TestCase(DefaultNumberWithColon + " ", null)]
        [TestCase(DefaultText + " ", null)]
        [TestCase(DefaultEnDate + " ", null)]
        public void TryParse_GiveInput_OutMachedValaue(string input, TestEnum? expected)
        {
            ConsoleParser.TryParse(input, typeof(TestEnum), out object? actual);

            actual.Should().Be(expected);
        }

        [TestCase(DefaultOne, true)]
        [TestCase(NotExistedEnumNumber, true)]
        [TestCase(DefaultNumberWithColon, false)]
        [TestCase(DefaultText, false)]
        [TestCase(DefaultEnDate, false)]
        [TestCase(" " + DefaultOne, true)]
        [TestCase(" " + NotExistedEnumNumber, true)]
        [TestCase(" " + DefaultNumberWithColon, false)]
        [TestCase(" " + DefaultText, false)]
        [TestCase(" " + DefaultEnDate, false)]
        [TestCase(DefaultOne + " ", true)]
        [TestCase(NotExistedEnumNumber + " ", true)]
        [TestCase(DefaultNumberWithColon + " ", false)]
        [TestCase(DefaultText + " ", false)]
        [TestCase(DefaultEnDate + " ", false)]
        public void TryParseForEnum_GiveInput_ReturnValaue(string input, bool expected)
        {
            var actual = ConsoleParser.TryParse(input, typeof(TestEnum), out object? result);

            actual.Should().Be(expected);
        }

        [Test]
        public void TryParseForObject_GiveInput_OutMachedValaue()
        {
            ConsoleParser.TryParse(Fixture.Create<string>(), typeof(ConsoleParserTests), out object? actual);

            actual.Should().BeNull();
        }

        [Test]
        public void TryParseForObject_GiveInput_ReturnValaue()
        {
            var actual = ConsoleParser.TryParse(Fixture.Create<string>(), typeof(ConsoleParserTests), out object? result);

            actual.Should().BeFalse();
        }
    }
}
