namespace BusinessSolutionChatGpt.Console.Infrastructure.Interfaces
{
    internal interface IPromptLabelFactory
    {
        string Create(string label, object? current, bool isNullable);
    }
}
