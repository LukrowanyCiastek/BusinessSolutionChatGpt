namespace BusinessSolutionChatGpt.Console.Infrastructure.Interfaces
{
    public interface IPromptLabelFactory
    {
        string Create(string label, object? current, bool isNullable);
    }
}
