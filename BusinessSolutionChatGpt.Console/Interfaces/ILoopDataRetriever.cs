using BusinessSolutionChatGpt.Console.Infrastructure.Interfaces;
using FluentValidation;

namespace BusinessSolutionChatGpt.Console.Interfaces
{
    internal interface ILoopDataRetriever<T>
    {
        T ReadObject(IValidator<T>? validator, IOutput output);

        T ReadPrimitive(IValidator<T>? validator, IOutput output, string instruction);

    }
}
