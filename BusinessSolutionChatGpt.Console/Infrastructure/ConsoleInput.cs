using System;
using BusinessSolutionChatGpt.Console.Infrastructure.Interfaces;

namespace BusinessSolutionChatGpt.Console.Infrastructure
{
    internal class ConsoleInput : IInput
    {
        public ConsoleKeyInfo ReadKey() => System.Console.ReadKey();

        public string? ReadLine() => System.Console.ReadLine();
    }
}
