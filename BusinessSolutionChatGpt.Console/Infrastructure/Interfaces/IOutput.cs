namespace BusinessSolutionChatGpt.Console.Infrastructure.Interfaces
{
    internal interface IOutput
    {
        void WriteLineWithEscape(string message);

        void WriteLine(string message);
    }
}
