using BusinessSolutionChatGpt.Infrastructure.Interfacrs;

namespace BusinessSolutionChatGpt.Infrastructure
{
    internal class ConsoleInput : IInput
    {
        public ConsoleKeyInfo ReadKey() => Console.ReadKey();

        public string? ReadLine() => Console.ReadLine();
    }
}
