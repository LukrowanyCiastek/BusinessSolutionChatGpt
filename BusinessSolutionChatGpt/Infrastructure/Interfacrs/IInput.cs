namespace BusinessSolutionChatGpt.Infrastructure.Interfacrs
{
    internal interface IInput
    {
        string? ReadLine();

        ConsoleKeyInfo ReadKey();
    }
}
