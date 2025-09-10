using AutoFixture;

namespace BusinessSolutionChatGpt.Tests.Core
{
    public static class AutoFixtureExtension
    {
        public static T FreezeMock<T>(this IFixture fixture) => fixture.Freeze<T>();
    }
}
