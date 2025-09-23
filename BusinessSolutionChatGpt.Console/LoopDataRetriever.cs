using BusinessSolutionChatGpt.Console.Infrastructure.Interfaces;
using BusinessSolutionChatGpt.Console.Interfaces;
using FluentValidation;
using Spectre.Console;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace BusinessSolutionChatGpt.Console
{
    internal class LoopDataRetriever<T> : ILoopDataRetriever<T>
        where T : new()
    {
        private readonly IPromptFactory promptFactory;

        public LoopDataRetriever(IPromptFactory promptFactory)
        {
            this.promptFactory = promptFactory;
        }

        T ILoopDataRetriever<T>.ReadObject(IValidator<T>? validator, IOutput output)
            => (T)ReadObject(typeof(T), validator, output);

        T ILoopDataRetriever<T>.ReadPrimitive(IValidator<T>? validator, IOutput output, string instruction)
            => (T)ReadPrimitive(typeof(T), validator, output, instruction);

        private object ReadPrimitive(Type type, IValidator<T>? validator, IOutput output, string instruction)
        {
            if (!type.IsPrimitive && !type.IsValueType)
            {
                throw new InvalidOperationException("Type is object primitive");
            }

            output.WriteLine(string.Empty);

            bool isCorrect;
            object? obj;
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

        private object ReadObject(Type type, IValidator<T>? validator, IOutput output)
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

        private object? ReadValue(string label, Type type, object? current)
        {
            var underlying = Nullable.GetUnderlyingType(type);
            var isNullable = underlying != null || !type.IsValueType;
            var t = underlying ?? type;

            if (t == typeof(string)) return ReadString(label, current, isNullable, promptFactory);
            if (t == typeof(bool)) return ReadBool(label, current, isNullable, promptFactory);
            if (t.IsEnum) return ReadEnum(label, current, isNullable, t, promptFactory);
            if (t == typeof(DateTime)) return LoopRead<DateTime>(label, current, isNullable, "[dim](format: yyyy-MM-dd)[/]", "[red]Nieprawidłowa data. Użyj yyyy-MM-dd[/]", promptFactory);
            if (t == typeof(Guid)) return LoopRead<Guid>(label, current, isNullable, "[dim](np. 00000000-0000-0000-0000-000000000000)[/]", "[red]Nieprawidłowy Guid[/]", promptFactory);
            if (t == typeof(int)) return LoopRead<int>(label, current, isNullable, "[dim](liczba całkowita)[/]", "[red]Nieprawidłowa liczba całkowita[/]", promptFactory);
            if (t == typeof(long)) return LoopRead<long>(label, current, isNullable, "[dim](liczba całkowita)[/]", "[red]Nieprawidłowa liczba całkowita[/]", promptFactory);
            if (t == typeof(float)) return LoopRead<float>(label, current, isNullable, "[dim](liczba, np. 123.45)[/]", "[red]Nieprawidłowa liczba[/]", promptFactory);
            if (t == typeof(double)) return LoopRead<double>(label, current, isNullable, "[dim](liczba, np. 123.45)[/]", "[red]Nieprawidłowa liczba[/]", promptFactory);
            if (t == typeof(decimal)) return LoopRead<decimal>(label, current, isNullable, "[dim](liczba, np. 123.45)[/]", "[red]Nieprawidłowa liczba[/]", promptFactory);

            throw new NotImplementedException("Not defined type for read");
        }

        public static object? ReadEnum(string label, object? current, bool isNullable, Type type, IPromptFactory promptFactory)
        {
            var names = isNullable ? [] : Enum.GetNames(type);
            var prompt = promptFactory.CreateSelectionPrompt(label, current, isNullable, 10, names);

            ConsoleParser.TryParse(AnsiConsole.Prompt(prompt), type, out var result);
            return result;
        }

        public static object? ReadBool(string label, object? current, bool isNullable, IPromptFactory promptFactory)
        {
            var prompt = promptFactory.CreateSelectionPrompt(label, current, isNullable, pageSize: null, "(puste)", "tak", "nie");
            var choice = AnsiConsole.Prompt(prompt);

            ConsoleParser.TryParse(choice, typeof(bool), out var result);

            return result;
        }

        private static object? ReadString(string label, object? current, bool isNullable, IPromptFactory promptFactory)
        {
            ConsoleParser.TryParse(AnsiConsole.Prompt(promptFactory.CreateSelectionPrompt(label, current, isNullable)), typeof(string), out var result);

            return result;
        }

        private static object? LoopRead<TIn>(string label, object? current, bool isNullable, string title, string warning, IPromptFactory promptFactory)
        {
            var prompt = promptFactory.CreateTextPrompt(label + " " + title, current, isNullable);

            if (!ConsoleParser.TryParse(AnsiConsole.Prompt(prompt), typeof(TIn), out var result))
            {
                AnsiConsole.MarkupLine(warning);
            }

            return result;
        }

        private static string? GetDisplayName(PropertyInfo p)
            => p.GetCustomAttribute<DisplayAttribute>()?.Name;

    }
}