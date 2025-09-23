using System.Globalization;

namespace BusinessSolutionChatGpt.Console
{
    internal sealed class ConsoleParser
    {
        internal static bool TryParseBool(string input, out object? value)
        {
            var s = input.ToLowerInvariant();
            if (s is "t" or "tak" or "y" or "yes" or "true" or "1")
            {
                value = true;
                return true;
            }

            if (s is "n" or "nie" or "no" or "false" or "0")
            {
                value = false;
                return true;
            }

            value = null;
            return false;
        }

        internal static bool TryParseString(string input, out object? value)
        {
            value = input;
            return true;
        }

        internal static bool TryParseInt(string input, out object? value)
        {
            if (int.TryParse(input, NumberStyles.Integer, CultureInfo.InvariantCulture, out var i))
            {
                value = i;
                return true;
            }

            value = null;
            return false;
        }

        internal static bool TryParseLong(string input, out object? value)
        {
            if (long.TryParse(input, NumberStyles.Integer, CultureInfo.InvariantCulture, out var l))
            {
                value = l;
                return true;
            }

            value = null;
            return false;
        }

        internal static bool TryParseDecimal(string input, out object? value)
        {
            if (decimal.TryParse(input, NumberStyles.Number, CultureInfo.InvariantCulture, out var d))
            {
                value = d;
                return true;
            }

            value = null;
            return false;
        }

        internal static bool TryParseFloat(string input, out object? value)
        {
            if (float.TryParse(input, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out var d))
            {
                value = d;
                return true;
            }
            value = null;
            return false;
        }

        internal static bool TryParseDouble(string input, out object? value)
        {
            if (double.TryParse(input, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out var d))
            {
                value = d;
                return true;
            }
            value = null;
            return false;
        }

        internal static bool TryParseDateTime(string input, out object? value)
        {
            if (string.IsNullOrEmpty(input))
            {
                value = null;
                return true;
            }

            if (DateTime.TryParseExact(input, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                                           DateTimeStyles.None, out var dt))
            {
                value = dt;
                return true;
            }
            if (DateTime.TryParse(input, CultureInfo.CurrentCulture, DateTimeStyles.AssumeLocal, out dt))
            {
                value = dt;
                return true;
            }
            value = null;
            return false;
        }

        internal static bool TryParseGuid(string input, out object? value)
        {
            if (string.IsNullOrEmpty(input))
            {
                value = null;
                return true;
            }

            if (Guid.TryParse(input, CultureInfo.CurrentCulture, out var dt))
            {
                value = dt;
                return true;
            }
            value = null;
            return false;
        }

        internal static bool TryParseEnum(string input, Type targetType, out object? value)
        {
            if (string.IsNullOrEmpty(input))
            {
                value = null;
                return true;
            }

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
            catch
            {
                value = null;
                return false;
            }
        }

        internal static bool TryParse(string input, Type targetType, out object? value)
        {
            input = input.Trim();

            if (targetType == typeof(bool))
            {
                return TryParseBool(input, out value);
            }

            if (targetType == typeof(string))
            {
                return TryParseString(input, out value);
            }

            if (targetType == typeof(int))
            {
                return TryParseInt(input, out value);
            }

            if (targetType == typeof(long))
            {
                return TryParseLong(input, out value);
            }

            if (targetType == typeof(decimal))
            {
                return TryParseDecimal(input, out value);
            }

            if (targetType == typeof(double))
            {
                return TryParseDouble(input, out value);
            }

            if(targetType == typeof(float))
            {
                return TryParseFloat(input, out value);
            }

            if (targetType == typeof(DateTime))
            {
                return TryParseDateTime(input, out value);
            }

            if (targetType == typeof(Guid))
            {
                return TryParseGuid(input, out value);
            }

            if (targetType.IsEnum)
            {
                return TryParseEnum(input, targetType, out value);
            }

            value = null;
            return false;
        }
    }
}
