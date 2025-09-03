using BusinessSolutionChatGpt.DTO.Input;
using BusinessSolutionChatGpt.Infrastructure.Interfaces;
using BusinessSolutionChatGpt.Interfaces;
using FluentValidation;
using FluentValidation.Results;

namespace BusinessSolutionChatGpt
{
    internal class LoopDataRetriever<T> : IInputRetriever<T>
    {
        private readonly IOutput output;
        private readonly IInput input;
        private readonly string? instruction;
        private readonly AbstractValidator<InputDTO<T>> dataValidator;

        public LoopDataRetriever(IOutput output,
            IInput input,
            AbstractValidator<InputDTO<T>> dataValidator,
            string? instruction)
        {
            this.instruction = instruction;
            this.output = output;
            this.input = input;
            this.dataValidator = dataValidator;
        }

        T? IInputRetriever<T>.TryGet()
        {
            ValidationResult dataValidationResult;
            T? result = default;
            do
            {
                output.WriteLineWithEscape(instruction!);

                var dto = new InputDTO<T>
                {
                    Raw = input.ReadLine()
                };

                dataValidationResult = dataValidator.Validate(dto);

                if (!dataValidationResult.IsValid)
                {
                    output.WriteLineWithEscape(dataValidationResult.Errors.First().ErrorMessage);
                }
                else
                {
                    result = dto.Value;
                }
            }
            while (!dataValidationResult.IsValid);

            return result;
        }
    }
}
