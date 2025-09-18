using BusinessSolutionChatGpt.Console.Infrastructure.Interfaces;
using FluentValidation;

namespace BusinessSolutionChatGpt.Console.Infrastructure
{
    internal class ConsoleInput : IInput
    {
        ConsoleKeyInfo IInput.ReadKey() => System.Console.ReadKey();

        string? IInput.ReadLine() => System.Console.ReadLine();

        public T ReadObject<T>(IValidator<T>? validator, IOutput output)
            where T : new() => LoopDataRetriever<T>.ReadObject(validator, output);

        public T ReadPrimitive<T>(IValidator<T>? validator, IOutput output, string instruction)
            where T : new() => LoopDataRetriever<T>.ReadPrimitive(validator, output, instruction);
    }
}
