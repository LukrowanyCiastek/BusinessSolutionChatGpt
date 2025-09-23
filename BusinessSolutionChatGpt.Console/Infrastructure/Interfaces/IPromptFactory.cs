using Spectre.Console;

namespace BusinessSolutionChatGpt.Console.Infrastructure.Interfaces
{
    public interface IPromptFactory
    {
        IPrompt<string> CreateTextPrompt(string label, object? current, bool isNullable);

        IPrompt<string> CreateSelectionPrompt(string label, object? current, bool isNullable);

        IPrompt<string> CreateSelectionPrompt(string label, object? current, bool isNullable, int? pageSize = null, params string[] choices);
    }
}
