using System.Globalization;

namespace BusinessSolutionChatGpt.Console
{
    internal sealed class ConsoleParser
    {
        public static bool TryParse(string input, Type targetType, out object? value)
        {
            input = input.Trim();

            if (targetType == typeof(bool))
            {
                var s = input.ToLowerInvariant();
                if (s is "t" or "tak" or "y" or "yes" or "true" or "1") { value = true; return true; }
                if (s is "n" or "nie" or "no" or "false" or "0") { value = false; return true; }
                value = null; return false;
            }

            if (targetType == typeof(string)) { value = input; return true; }

            if (targetType == typeof(int))
            {
                if (int.TryParse(input, NumberStyles.Integer, CultureInfo.InvariantCulture, out var i))
                { value = i; return true; }
                value = null; return false;
            }

            if (targetType == typeof(long))
            {
                if (long.TryParse(input, NumberStyles.Integer, CultureInfo.InvariantCulture, out var l))
                { value = l; return true; }
                value = null; return false;
            }

            if (targetType == typeof(decimal))
            {
                if (decimal.TryParse(input, NumberStyles.Number, CultureInfo.InvariantCulture, out var d))
                { value = d; return true; }
                value = null; return false;
            }

            if (targetType == typeof(double))
            {
                if (double.TryParse(input, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out var d))
                { value = d; return true; }
                value = null; return false;
            }

            if (targetType == typeof(DateTime))
            {
                if (DateTime.TryParseExact(input, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                                           DateTimeStyles.None, out var dt))
                { value = dt; return true; }
                if (DateTime.TryParse(input, CultureInfo.CurrentCulture, DateTimeStyles.AssumeLocal, out dt))
                { value = dt; return true; }
                value = null; return false;
            }

            if (targetType.IsEnum)
            {
                // nazwa (case-insensitive) lub liczba
                if (int.TryParse(input, out var enumInt) && Enum.IsDefined(targetType, enumInt))
                {
                    value = Enum.ToObject(targetType, enumInt);
                    return true;
                }
                try
                {
                    value = Enum.Parse(targetType, input, ignoreCase: true);
                    return true;
                }
                catch { value = null; return false; }
            }

            // Fallback na ChangeType (działa m.in. dla Guid)
            try
            {
                value = Convert.ChangeType(input, targetType, CultureInfo.InvariantCulture);
                return true;
            }
            catch { value = null; return false; }
        }
    }
}
