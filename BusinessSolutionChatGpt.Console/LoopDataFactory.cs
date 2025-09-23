using BusinessSolutionChatGpt.Console.Infrastructure.Interfaces;
using BusinessSolutionChatGpt.Console.Interfaces;

namespace BusinessSolutionChatGpt.Console
{
    internal class LoopDataFactory : ILoopDataFactory
    {
        private readonly IPromptFactory promptFactory;

        public LoopDataFactory(IPromptFactory promptFactory)
        {
            this.promptFactory = promptFactory;
        }

        public ILoopDataRetriever<T> Create<T>() where T : new()  => new LoopDataRetriever<T>(promptFactory);
    }
}
