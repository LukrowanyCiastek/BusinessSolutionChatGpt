namespace BusinessSolutionChatGpt.Console.Infrastructure.Interfaces
{
    internal interface IInput
    {
        string? ReadLine();

        ConsoleKeyInfo ReadKey();
    }
}
