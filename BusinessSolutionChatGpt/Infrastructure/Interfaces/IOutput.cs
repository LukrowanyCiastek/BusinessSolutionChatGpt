namespace BusinessSolutionChatGpt.Infrastructure.Interfaces
{
    internal interface IOutput
    {
        void WriteLineWithEscape(string message);

        void WriteLine(string message);
    }
}
