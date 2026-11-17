using System.Globalization;
using System.Reflection;
using ClinicApp.Attributes;

namespace ClinicApp.Utils;

public static class FormBuilder
{
    public static T Build<T>() where T : new()
    {
        T result = new T();

        foreach (PropertyInfo prop in typeof(T).GetProperties())
        {
            if (prop.GetSetMethod() == null)
            {
                continue;
            }

            string prompt = BuildPrompt(prop);

            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine() ?? "";

                try
                {
                    prop.SetValue(result, ConvertInput(input, prop.PropertyType));
                    break;
                }
                catch (Exception)
                {
                    Console.WriteLine("Некоректне значення, спробуйте ще раз.");
                }
            }
        }

        return result;
    }

    private static string BuildPrompt(PropertyInfo prop)
    {
        string hints = "";

        if (prop.GetCustomAttribute<RequiredAttribute>() != null)
        {
            hints += "обов'язкове";
        }

        MaxLengthAttribute? maxLength = prop.GetCustomAttribute<MaxLengthAttribute>();
        if (maxLength != null)
        {
            hints += (hints.Length > 0 ? ", " : "") + $"макс. {maxLength.Length}";
        }

        MinValueAttribute? minValue = prop.GetCustomAttribute<MinValueAttribute>();
        if (minValue != null)
        {
            hints += (hints.Length > 0 ? ", " : "") + $"мін. {minValue.Min.ToString(CultureInfo.InvariantCulture)}";
        }

        if (hints.Length == 0)
        {
            return $"{prop.Name}: ";
        }

        return $"{prop.Name} ({hints}): ";
    }

    private static object? ConvertInput(string input, Type type)
    {
        Type target = Nullable.GetUnderlyingType(type) ?? type;

        if (target.IsEnum)
        {
            return Enum.Parse(target, input, true);
        }

        return Convert.ChangeType(input, target, CultureInfo.InvariantCulture);
    }
}
