using BusinessSolutionChatGpt.Console.Infrastructure.Interfaces;
using Spectre.Console;

namespace BusinessSolutionChatGpt.Console.Infrastructure
{
    internal class PromptFactory : IPromptFactory
    {
        private readonly IPromptLabelFactory promptLabelFactory;
        private const string NullText = "(puste)";

        public PromptFactory(IPromptLabelFactory promptLabelFactory)
        {
            this.promptLabelFactory = promptLabelFactory;
        }

        IPrompt<string> IPromptFactory.CreateTextPrompt(string label, object? current, bool isNullable)
        {
            var prompt = new TextPrompt<string>(this.promptLabelFactory.Create(label, current, isNullable));
            if (isNullable)
            {
                prompt.AllowEmpty();
            }

            return prompt;
        }

        IPrompt<string> IPromptFactory.CreateSelectionPrompt(string label, object? current, bool isNullable)
            => new SelectionPrompt<string>().Title(this.promptLabelFactory.Create(label, current, isNullable));

        IPrompt<string> IPromptFactory.CreateSelectionPrompt(string label, object? current, bool isNullable, int? pageSize, params string[] choices)
        {
            SelectionPrompt<string>? prompt = ((IPromptFactory)this).CreateSelectionPrompt(label, current, isNullable) as SelectionPrompt<string>;

            if (isNullable) {
                if(choices == null)
                {
                    throw new ArgumentNullException(nameof(choices));
                }

                var extendedChoices = choices.ToList();
                extendedChoices.Add(NullText);
                prompt?.AddChoices(choices: [.. extendedChoices]);
            }
            else {
                prompt?.AddChoices(choices);
            }

            return pageSize is not null ? prompt?.PageSize(pageSize.Value)! : prompt!;
        }
    }
}
