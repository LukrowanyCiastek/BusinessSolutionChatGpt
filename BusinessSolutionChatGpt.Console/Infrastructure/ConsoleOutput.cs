using BusinessSolutionChatGpt.Console.Infrastructure.Interfaces;

namespace BusinessSolutionChatGpt.Console.Infrastructure
{
    internal class ConsoleOutput : IOutput
    {
        public void WriteLine(string message)
        {
            System.Console.WriteLine(message);
        }

        void IOutput.WriteLineWithEscape(string message)
        {
            System.Console.WriteLine(string.Empty);
            WriteLine(message);
        }
    }
}
