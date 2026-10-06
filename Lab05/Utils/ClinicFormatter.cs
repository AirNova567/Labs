using ClinicApp.Enums;

namespace ClinicApp.Utils;

public static class ClinicFormatter
{
    public static string FormatBloodType(BloodType bloodType)
    {
        switch (bloodType)
        {
            case BloodType.APositive:
                return "A+";
            case BloodType.ANegative:
                return "A-";
            case BloodType.BPositive:
                return "B+";
            case BloodType.BNegative:
                return "B-";
            case BloodType.ABPositive:
                return "AB+";
            case BloodType.ABNegative:
                return "AB-";
            case BloodType.OPositive:
                return "O+";
            case BloodType.ONegative:
                return "O-";
            default:
                return "Невідомо";
        }
    }

    public static string FormatSpeciality(Speciality speciality)
    {
        switch (speciality)
        {
            case Speciality.Cardiology:
                return "Кардіологія";
            case Speciality.Neurology:
                return "Неврологія";
            case Speciality.Pediatrics:
                return "Педіатрія";
            case Speciality.Surgery:
                return "Хірургія";
            case Speciality.Orthopedics:
                return "Ортопедія";
            case Speciality.Dermatology:
                return "Дерматологія";
            case Speciality.Emergency:
                return "Невідкладна допомога";
            default:
                return "Загальна практика";
        }
    }

    public static string FormatAge(int age)
    {
        int lastTwo = age % 100;
        int last = age % 10;
        string word;

        if (lastTwo >= 11 && lastTwo <= 14)
        {
            word = "років";
        }
        else if (last == 1)
        {
            word = "рік";
        }
        else if (last >= 2 && last <= 4)
        {
            word = "роки";
        }
        else
        {
            word = "років";
        }

        return $"{age} {word}";
    }

    public static string FormatPhone(string phone)
    {
        if (phone.Length != 10)
        {
            return phone;
        }

        return $"({phone.Substring(0, 3)}) {phone.Substring(3, 3)}-{phone.Substring(6)}";
    }
}
