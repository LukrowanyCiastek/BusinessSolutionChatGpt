using BusinessSolutionChatGpt.Console.Infrastructure.Interfaces;
using BusinessSolutionChatGpt.Console.Interfaces;
using FluentValidation;

namespace BusinessSolutionChatGpt.Console.Infrastructure
{
    internal class ConsoleInput : IInput
    {
        private readonly ILoopDataFactory loopDataFactory;

        public ConsoleInput(ILoopDataFactory loopDataFactory)
        {
            this.loopDataFactory = loopDataFactory;
        }

        ConsoleKeyInfo IInput.ReadKey() => System.Console.ReadKey();

        string? IInput.ReadLine() => System.Console.ReadLine();

        T IInput.ReadObject<T>(IValidator<T>? validator, IOutput output) =>
            loopDataFactory.Create<T>().ReadObject(validator, output);

        T IInput.ReadPrimitive<T>(IValidator<T>? validator, IOutput output, string instruction) =>
            loopDataFactory.Create<T>().ReadPrimitive(validator, output, instruction);
    }
}
