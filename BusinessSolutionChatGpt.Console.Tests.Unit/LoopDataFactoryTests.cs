using AutoFixture;
using BusinessSolutionChatGpt.Console.Interfaces;
using BusinessSolutionChatGpt.Tests.Core;
using FluentAssertions;

namespace BusinessSolutionChatGpt.Console.Tests.Unit
{
    [TestFixture]
    public class LoopDataFactoryTests : BaseFixture
    {
        [Test]
        public void Create_Called_CreatesLoopDataRetriever()
        {
            ILoopDataFactory factory = Fixture.Create<LoopDataFactory>();

            var actual = factory.Create<int>();

            actual.Should().BeOfType<LoopDataRetriever<int>>();
        }
    }
}
