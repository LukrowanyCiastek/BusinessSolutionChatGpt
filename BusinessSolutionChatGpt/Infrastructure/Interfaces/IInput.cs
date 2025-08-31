namespace BusinessSolutionChatGpt.Infrastructure.Interfaces
{
    internal interface IInput
    {
        string? ReadLine();

        ConsoleKeyInfo ReadKey();
    }
}
