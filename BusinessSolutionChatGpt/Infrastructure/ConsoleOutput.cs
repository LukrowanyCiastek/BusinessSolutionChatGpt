using BusinessSolutionChatGpt.Infrastructure.Interfaces;

namespace BusinessSolutionChatGpt.Infrastructure
{
    internal class ConsoleOutput : IOutput
    {
        public void WriteLine(string message)
        {
            Console.WriteLine(message);
        }

        void IOutput.WriteLineWithEscape(string message)
        {
            Console.WriteLine(string.Empty);
            WriteLine(message);
        }
    }
}
