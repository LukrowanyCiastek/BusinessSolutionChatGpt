using AutoFixture;
using BusinessSolutionChatGpt.Console.Infrastructure.Interfaces;
using BusinessSolutionChatGpt.Tests.Core;
using FluentAssertions;

namespace BusinessSolutionChatGpt.Console.Tests.Unit
{
    [TestFixture]
    public class LoopDataRetrieverTests : BaseFixture
    {
        [Test]
        public void ReadPrimitive_GiveNotPrimitive_ThrowsInvalidOperationException()
        {
            var mockOutput = Fixture.FreezeMock<IOutput>();
            Action act = () => LoopDataRetriever<object>.ReadPrimitive(typeof(object), null, mockOutput, Fixture.Create<string>());

            act.Should()
               .Throw<InvalidOperationException>()
               .WithMessage("Type is not primitive");
        }
    }
}
