namespace BusinessSolutionChatGpt.Validators.Interfaces
{
    internal interface IValidator<T>
    {
        bool IsValid(T? input);
    }
}
