using FluentValidation;

namespace BusinessSolutionChatGpt.Console.Infrastructure.Interfaces
{
    public interface IInput
    {
        string? ReadLine();

        ConsoleKeyInfo ReadKey();

        T ReadObject<T>(IValidator<T>? validator, IOutput output) where T : new();

        T ReadPrimitive<T>(IValidator<T>? validator, IOutput output, string instruction) where T : new();
    }
}
