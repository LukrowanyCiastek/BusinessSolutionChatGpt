namespace BusinessSolutionChatGpt.Infrastructure.Interfacrs
{
    internal interface IOutput
    {
        void WriteLineWithEscape(string message);

        void WriteLine(string message);
    }
}
