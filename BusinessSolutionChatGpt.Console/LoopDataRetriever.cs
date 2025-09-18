using BusinessSolutionChatGpt.Console.Infrastructure.Interfaces;
using FluentValidation;
using Spectre.Console;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;

namespace BusinessSolutionChatGpt.Console
{
    internal static class LoopDataRetriever<T>
        where T : new()
    {

        internal static T ReadObject(IValidator<T>? validator, IOutput output)
        => (T)ReadObject(typeof(T), validator, output);

        internal static T ReadPrimitive(IValidator<T>? validator, IOutput output, string instruction)
        => (T)ReadPrimitive(typeof(T), validator, output, instruction);

        internal static object ReadPrimitive(Type type, IValidator<T>? validator, IOutput output, string instruction)
        {
            output.WriteLine(string.Empty);
            var obj = Activator.CreateInstance(type)!;

            var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                            .Where(p => p.CanWrite);

            if (!type.IsPrimitive)
            {
                throw new InvalidOperationException("Type is not primitive");
            }

            bool isCorrect = false;
            do
            {
                obj = ReadValue(instruction, type, null);

                if (validator == null)
                {
                    isCorrect = true;
                }
                else
                {
                    var validationResult = validator.Validate((T)obj!);
                    isCorrect = validationResult.IsValid;
                    if (!isCorrect)
                    {
                        foreach (var error in validationResult.Errors)
                        {
                            output.WriteLineWithEscape(error.ErrorMessage);
                        }
                    }
                }

            } while (!isCorrect);

            return obj!;
        }

        internal static object ReadObject(Type type, IValidator<T>? validator, IOutput output)
        {
            output.WriteLine(string.Empty);
            var obj = Activator.CreateInstance(type)!;

            var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                            .Where(p => p.CanWrite);

            if (type.IsPrimitive)
            {
                throw new InvalidOperationException("Type is primitive");
            }

            bool isCorrect = false;
            do
            {
                foreach (var p in props)
                {
                    var display = GetDisplayName(p) ?? p.Name;
                    var current = p.GetValue(obj);
                    var value = ReadValue(display, p.PropertyType, current);
                    p.SetValue(obj, value);
                }

                if (validator == null)
                {
                    isCorrect = true;
                }
                else
                {
                    var validationResult = validator.Validate((T)obj);
                    isCorrect = validationResult.IsValid;
                    if (!isCorrect)
                    {
                        foreach (var error in validationResult.Errors)
                        {
                            output.WriteLineWithEscape(error.ErrorMessage);
                        }
                    }
                }

            } while (!isCorrect);

            return obj;
        }

        private static object? ReadValue(string label, Type type, object? current)
        {
            var underlying = Nullable.GetUnderlyingType(type);
            var isNullable = underlying != null || !type.IsValueType;
            var t = underlying ?? type;

            if (t == typeof(string)) return ReadString(label, current, isNullable);
            if (t == typeof(bool)) return ReadBool(label, current, isNullable);
            if (t.IsEnum) return ReadEnum(label, current, isNullable, t);
            if (t == typeof(DateTime)) return LoopRead<DateTime>(label, current, isNullable, "[dim](format: yyyy-MM-dd)[/]", "[red]Nieprawidłowa data. Użyj yyyy-MM-dd[/]");
            if (t == typeof(Guid)) return LoopRead<Guid>(label, current, isNullable, "[dim](np. 00000000-0000-0000-0000-000000000000)[/]", "[red]Nieprawidłowy Guid[/]");
            if (t == typeof(int)) return LoopRead<int>(label, current, isNullable, "[dim](liczba całkowita)[/]", "[red]Nieprawidłowa liczba całkowita[/]");
            if (t == typeof(long)) return LoopRead<long>(label, current, isNullable, "[dim](liczba całkowita)[/]", "[red]Nieprawidłowa liczba całkowita[/]");
            if (t == typeof(float)) return LoopRead<float>(label, current, isNullable, "[dim](liczba, np. 123.45)[/]", "[red]Nieprawidłowa liczba[/]");
            if (t == typeof(double)) return LoopRead<double>(label, current, isNullable, "[dim](liczba, np. 123.45)[/]", "[red]Nieprawidłowa liczba[/]");
            if (t == typeof(decimal)) return LoopRead<decimal>(label, current, isNullable, "[dim](liczba, np. 123.45)[/]", "[red]Nieprawidłowa liczba[/]");
            if (IsSimple(t)) return LoopRead<object>(label, current, isNullable, string.Empty, "[red]Nieprawidłowa wartość dla typu {t.Name}[/]");

            return current;
        }

        public static object? ReadEnum(string label, object? current, bool isNullable, Type type)
        {
            var names = Enum.GetNames(type).ToList();
            var prompt = new SelectionPrompt<string>()
                .Title(PromptTitle(label, current, isNullable))
                .PageSize(10);

            if (isNullable)
                prompt.AddChoices(new[] { "(puste)" }.Concat(names));
            else
                prompt.AddChoices(names);

            ConsoleParser.TryParse(AnsiConsole.Prompt(prompt), type, out var result);
            return result;
        }

        public static object? ReadBool(string label, object? current, bool isNullable)
        {
            var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title(PromptTitle(label, current, isNullable))
                        .AddChoices("(puste)", "tak", "nie"));

            ConsoleParser.TryParse(choice, typeof(bool), out var result);

            return result;
        }

        private static object? ReadString(string label, object? current, bool isNullable)
        {
            var prompt = new TextPrompt<string>(PromptTitle(label, current, isNullable));
            if (isNullable)
            {
                prompt.AllowEmpty();
            }

            ConsoleParser.TryParse(AnsiConsole.Prompt(prompt), typeof(string), out var result);

            return result;
        }

        private static object? LoopRead<TIn>(string label, object? current, bool isNullable, string title, string warning)
        {
            while (true)
            {
                var prompt = new TextPrompt<string>(PromptTitle(label, current, isNullable) + " " + title);
                if (isNullable)
                {
                    prompt.AllowEmpty();
                }

                if (ConsoleParser.TryParse(AnsiConsole.Prompt(prompt), typeof(TIn), out var result))
                {
                    return result;
                }

                AnsiConsole.MarkupLine(warning);
            }
        }

        private static string PromptTitle(string label, object? current, bool isNullable)
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

        private static string? GetDisplayName(PropertyInfo p)
            => p.GetCustomAttribute<DisplayAttribute>()?.Name;

        private static bool IsSimple(Type t)
            => t.IsPrimitive
               || t.IsEnum
               || t == typeof(string)
               || t == typeof(decimal)
               || t == typeof(DateTime)
               || t == typeof(Guid);

    }
}