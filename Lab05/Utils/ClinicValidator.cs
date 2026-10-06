using System.Text.RegularExpressions;

namespace ClinicApp.Utils;

public static class ClinicValidator
{
    private const int MaxNameLength = 50;
    private const string PhonePattern = @"^[0-9]{10}\z";
    private const string EmailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+\z";
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
        if (phone == null || !Regex.IsMatch(phone, PhonePattern))
        {
            throw new ArgumentException("Телефон має містити рівно 10 цифр.", nameof(phone));
        }
    }

    public static void ValidateEmail(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            return;
        }

        if (!Regex.IsMatch(email, EmailPattern))
        {
            throw new ArgumentException("Некоректний формат email.", nameof(email));
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
