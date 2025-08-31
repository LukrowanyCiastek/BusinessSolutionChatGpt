using BusinessSolutionChatGpt.Infrastructure.Interfaces;
using BusinessSolutionChatGpt.Interfaces;
using BusinessSolutionChatGpt.Parsers.Interfaces;
using BusinessSolutionChatGpt.Validators.Interfaces;

namespace BusinessSolutionChatGpt
{
    internal class LoopDataRetriever<T> : IInputRetriever<T>
    {
        private readonly IOutput output;
        private readonly IInput input;
        private readonly string instruction;
        private readonly string? repeatMessage;
        private readonly IParser<T> parser;
        private readonly IValidator<string> messageValidator;
        private readonly IValidator<string> dataValidator;

        public LoopDataRetriever(IOutput output,
            IInput input,
            IValidator<string> messageValidator,
            IValidator<string> dataValidator,
            IParser<T> parser,
            string instruction,
            string? repeatMessage)
        {
            this.output = output;
            this.input = input;
            this.messageValidator = messageValidator;
            this.dataValidator = dataValidator;
            this.instruction = instruction;
            this.repeatMessage = repeatMessage;
            this.parser = parser;
        }

        T IInputRetriever<T>.TryGet()
        {
            bool isCorrect;
            T result;
            do
            {
                output.WriteLineWithEscape(instruction);

                string? rawData = input.ReadLine();
                isCorrect = dataValidator.IsValid(rawData);
                result = parser.Parse(rawData!);

                if (!isCorrect && messageValidator.IsValid(repeatMessage!))
                {
                    output.WriteLineWithEscape(repeatMessage!);
                }
            }
            while (!isCorrect);

            return result;
        }
    }
}
