using BusinessSolutionChatGpt.Console.Infrastructure.Interfaces;
using System.Globalization;

namespace BusinessSolutionChatGpt.Console.Infrastructure
{
    internal class PromptLabelFactory : IPromptLabelFactory
    {
        public string Create(string label, object? current, bool isNullable)
        {
            var defaultText = FormatDefaultLabel(current, isNullable);
            return $"[bold]{label}[/]{defaultText}";
        }

        private static string FormatDefaultLabel(object? current, bool isNullable)
        {
            if (isNullable) return " [dim](Enter = puste)[/]";
            if (current is null) return "";
            return $" [dim](domyślnie: {FormatValue(current)})[/]";
        }

        private static string FormatValue(object value)
            => value switch
            {
                DateTime dt => dt.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                bool b => b ? "tak" : "nie",
                IFormattable f => f.ToString(null, CultureInfo.InvariantCulture) ?? value.ToString() ?? "",
                _ => value.ToString() ?? ""
            };
    }
}
