namespace ClinicApp.Utils;

public static class ClinicValidator
{
    private const int MaxNameLength = 50;
    private const int PhoneLength = 10;
    private static readonly DateTime MinDate = new(1900, 1, 1);

    public static void ValidateName(string value, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException($"Поле {fieldName} не може бути порожнім.", fieldName);
        }

        if (value.Length > MaxNameLength)
        {
            throw new ArgumentException($"Поле {fieldName} не може бути довшим за {MaxNameLength} символів.", fieldName);
        }
    }

    public static void ValidatePhone(string phone)
    {
        if (phone == null || phone.Length != PhoneLength)
        {
            throw new ArgumentException($"Телефон має містити рівно {PhoneLength} цифр.", nameof(phone));
        }

        foreach (char c in phone)
        {
            if (c < '0' || c > '9')
            {
                throw new ArgumentException("Телефон має містити лише цифри.", nameof(phone));
            }
        }
    }

    public static void ValidateDate(DateTime value, string fieldName)
    {
        if (value > DateTime.Today)
        {
            throw new ArgumentOutOfRangeException(fieldName, value, $"Поле {fieldName} не може бути в майбутньому.");
        }

        if (value < MinDate)
        {
            throw new ArgumentOutOfRangeException(fieldName, value, $"Поле {fieldName} не може бути раніше {MinDate.Year} року.");
        }
    }

    public static void ValidatePositive(int value, string fieldName)
    {
        if (value <= 0)
        {
            throw new ArgumentOutOfRangeException(fieldName, value, $"Поле {fieldName} має бути більшим за 0.");
        }
    }
}
