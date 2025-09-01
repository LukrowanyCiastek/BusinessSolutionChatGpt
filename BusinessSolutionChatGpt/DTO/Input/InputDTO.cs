namespace BusinessSolutionChatGpt.DTO.Input
{
    public class InputDTO<TOut>
    {
        public string? Raw {  get; set; }

        public TOut? Value { get; set; }
    }
}
