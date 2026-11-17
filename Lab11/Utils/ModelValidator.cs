using System.Reflection;
using ClinicApp.Attributes;

namespace ClinicApp.Utils;

public static class ModelValidator
{
    public static ValidationResult Validate(object obj)
    {
        ValidationResult result = new ValidationResult();

        foreach (PropertyInfo prop in obj.GetType().GetProperties())
        {
            object? value = prop.GetValue(obj);

            RequiredAttribute? required = prop.GetCustomAttribute<RequiredAttribute>();
            if (required != null)
            {
                bool missing = value == null || (value is string text && string.IsNullOrWhiteSpace(text));
                if (missing)
                {
                    result.AddError($"{prop.Name}: {required.ErrorMessage}");
                }
            }

            MaxLengthAttribute? maxLength = prop.GetCustomAttribute<MaxLengthAttribute>();
            if (maxLength != null && value is string str && str.Length > maxLength.Length)
            {
                string message = maxLength.ErrorMessage.Length > 0
                    ? maxLength.ErrorMessage
                    : $"Length must not exceed {maxLength.Length} characters.";
                result.AddError($"{prop.Name}: {message}");
            }

            MinValueAttribute? minValue = prop.GetCustomAttribute<MinValueAttribute>();
            if (minValue != null && value != null && IsNumeric(value) && Convert.ToDouble(value) < minValue.Min)
            {
                string message = minValue.ErrorMessage.Length > 0
                    ? minValue.ErrorMessage
                    : $"Value must be at least {minValue.Min}.";
                result.AddError($"{prop.Name}: {message}");
            }
        }

        return result;
    }

    public static void PrintInfo(Type type)
    {
        PropertyInfo[] properties = type.GetProperties();

        int nameWidth = 0;
        foreach (PropertyInfo prop in properties)
        {
            if (prop.Name.Length > nameWidth)
            {
                nameWidth = prop.Name.Length;
            }
        }

        Console.WriteLine(type.Name);
        foreach (PropertyInfo prop in properties)
        {
            string line = "  " + prop.Name.PadRight(nameWidth);
            foreach (Attribute attribute in prop.GetCustomAttributes())
            {
                string attributeName = attribute.GetType().Name;
                if (attributeName.EndsWith("Attribute"))
                {
                    attributeName = attributeName.Substring(0, attributeName.Length - "Attribute".Length);
                }

                line += $"  [{attributeName}]";
            }

            Console.WriteLine(line);
        }
    }

    private static bool IsNumeric(object value)
    {
        return value is byte || value is short || value is int || value is long
            || value is float || value is double || value is decimal;
    }
}
