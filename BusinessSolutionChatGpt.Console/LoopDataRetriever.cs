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

        internal static T ReadObject(AbstractValidator<T>? validator, IOutput output)
        => (T)ReadObject(typeof(T), validator, output);

        internal static T ReadPrimitive(AbstractValidator<T>? validator, IOutput output, string instruction)
        => (T)ReadPrimitive(typeof(T), validator, output, instruction);

        internal static object ReadPrimitive(Type type, AbstractValidator<T>? validator, IOutput output, string instruction)
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

        internal static object ReadObject(Type type, AbstractValidator<T>? validator, IOutput output)
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

            // STRING
            if (t == typeof(string))
            {
                var prompt = new TextPrompt<string>(PromptTitle(label, current, isNullable));
                if (isNullable)
                {
                    prompt.AllowEmpty();
                }
                return AnsiConsole.Prompt(prompt);
            }

            // BOOL (Confirm dla nie-null, 3-pozycyjny wybór dla nullable)
            if (t == typeof(bool))
            {
                if (isNullable)
                {
                    var choice = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .Title(PromptTitle(label, current, isNullable))
                            .AddChoices("(puste)", "tak", "nie"));
                    return choice switch
                    {
                        "tak" => true,
                        "nie" => (bool?)false,
                        _ => null
                    };
                }
                else
                {
                    bool defaultVal = current is bool b && b;
                    // Confirm ma wbudowany domyślny wybór
                    return AnsiConsole.Confirm(PromptTitle(label, current, isNullable), defaultVal);
                }
            }

            // ENUM
            if (t.IsEnum)
            {
                var names = Enum.GetNames(t).ToList();
                var prompt = new SelectionPrompt<string>()
                    .Title(PromptTitle(label, current, isNullable))
                    .PageSize(10);

                if (isNullable)
                    prompt.AddChoices(new[] { "(puste)" }.Concat(names));
                else
                    prompt.AddChoices(names);

                var selected = AnsiConsole.Prompt(prompt);
                if (isNullable && selected == "(puste)") return null;

                return Enum.Parse(t, selected, ignoreCase: true);
            }

            // DATETIME o ustalonym formacie
            if (t == typeof(DateTime))
            {
                while (true)
                {
                    var prompt = new TextPrompt<string>(PromptTitle(label, current, isNullable) + " [dim](format: yyyy-MM-dd)[/]");
                    if (isNullable)
                    {
                        prompt.AllowEmpty();
                    }
                    var text = AnsiConsole.Prompt(prompt);

                    if (string.IsNullOrWhiteSpace(text))
                        return isNullable ? null : default(DateTime);

                    if (DateTime.TryParseExact(text, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                                               DateTimeStyles.None, out var dt))
                        return dt;

                    AnsiConsole.MarkupLine("[red]Nieprawidłowa data. Użyj yyyy-MM-dd[/]");
                }
            }

            // GUID
            if (t == typeof(Guid))
            {
                while (true)
                {
                    var prompt = new TextPrompt<string>(PromptTitle(label, current, isNullable) + " [dim](np. 00000000-0000-0000-0000-000000000000)[/]");
                    if (isNullable)
                    {
                        prompt.AllowEmpty();
                    }
                    var text = AnsiConsole.Prompt(prompt);

                    if (string.IsNullOrWhiteSpace(text))
                        return isNullable ? null : Guid.Empty;

                    if (Guid.TryParse(text, out var g))
                        return g;

                    AnsiConsole.MarkupLine("[red]Nieprawidłowy Guid[/]");
                }
            }

            // LICZBY
            if (t == typeof(int)) return ReadNumber<int>(label, current, isNullable, int.TryParse);
            if (t == typeof(long)) return ReadNumber<long>(label, current, isNullable, long.TryParse);
            if (t == typeof(float)) return ReadFloat<float>(label, current, isNullable, float.TryParse);
            if (t == typeof(double)) return ReadFloat<double>(label, current, isNullable, double.TryParse);
            if (t == typeof(decimal)) return ReadDecimal(label, current, isNullable);

            // PROSTE TYPY – podejdź generycznie
            if (IsSimple(t))
            {
                while (true)
                {
                    var prompt = new TextPrompt<string>(PromptTitle(label, current, isNullable));
                    if (isNullable)
                    {
                        prompt.AllowEmpty();
                    }
                    var text = AnsiConsole.Prompt(prompt);

                    if (string.IsNullOrWhiteSpace(text))
                        return isNullable ? null : Activator.CreateInstance(t);

                    try
                    {
                        var val = Convert.ChangeType(text, t, CultureInfo.InvariantCulture);
                        return val;
                    }
                    catch
                    {
                        AnsiConsole.MarkupLine($"[red]Nieprawidłowa wartość dla typu {t.Name}[/]");
                    }
                }
            }

            // fallback
            return current;
        }

        private static object? ReadNumber<T>(string label, object? current, bool isNullable,
        TryParseNumber<T> tryParse) where T : struct
        {
            while (true)
            {
                var prompt = new TextPrompt<string>(PromptTitle(label, current, isNullable) + " [dim](liczba całkowita)[/]");
                if (isNullable)
                {
                    prompt.AllowEmpty();
                }
                var text = AnsiConsole.Prompt(prompt);

                if (string.IsNullOrWhiteSpace(text))
                    return isNullable ? null : default(T);

                if (tryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out var val))
                    return val;

                AnsiConsole.MarkupLine("[red]Nieprawidłowa liczba całkowita[/]");
            }
        }

        private static object? ReadFloat<T>(string label, object? current, bool isNullable,
            TryParseFloat<T> tryParse) where T : struct
        {
            while (true)
            {
                var prompt = new TextPrompt<string>(PromptTitle(label, current, isNullable) + " [dim](liczba, np. 123.45)[/]");
                if (isNullable)
                {
                    prompt.AllowEmpty();
                }
                var text = AnsiConsole.Prompt(prompt);

                if (string.IsNullOrWhiteSpace(text))
                    return isNullable ? null : default(T);

                if (tryParse(text, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out var val))
                    return val;

                AnsiConsole.MarkupLine("[red]Nieprawidłowa liczba[/]");
            }
        }

        private static object? ReadDecimal(string label, object? current, bool isNullable)
        {
            while (true)
            {
                var prompt = new TextPrompt<string>(PromptTitle(label, current, isNullable) + " [dim](liczba, np. 123.45)[/]");
                if (isNullable)
                {
                    prompt.AllowEmpty();
                }
                var text = AnsiConsole.Prompt(prompt);

                if (string.IsNullOrWhiteSpace(text))
                    return isNullable ? null : default(decimal);

                if (decimal.TryParse(text, NumberStyles.Number, CultureInfo.InvariantCulture, out var val))
                    return val;

                AnsiConsole.MarkupLine("[red]Nieprawidłowa liczba[/]");
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

        private delegate bool TryParseNumber<T>(string s, NumberStyles style, IFormatProvider provider, out T value);
        private delegate bool TryParseFloat<T>(string s, NumberStyles style, IFormatProvider provider, out T value);
    }
}