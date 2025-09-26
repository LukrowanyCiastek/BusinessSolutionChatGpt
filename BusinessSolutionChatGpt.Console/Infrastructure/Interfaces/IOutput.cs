namespace BusinessSolutionChatGpt.Console.Infrastructure.Interfaces
{
    public interface IOutput
    {
        void WriteLineWithEscape(string message);

        void WriteLine(string message);
    }
}
