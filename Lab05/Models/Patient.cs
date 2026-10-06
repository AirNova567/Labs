using ClinicApp.Enums;
using ClinicApp.Utils;

namespace ClinicApp.Models;

public class Patient
{
    private static int _nextId = 1;
    public int Id { get; }
    private string _firstName = "";
    private string _lastName = "";
    private DateTime _dateOfBirth;
    private string _phone = "";

    public string FirstName
    {
        get => _firstName;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Ім'я не може бути порожнім.", nameof(FirstName));
            }

            if (value.Length > 50)
            {
                throw new ArgumentException("Ім'я не може бути довшим за 50 символів.", nameof(FirstName));
            }

            _firstName = value;
        }
    }

    public string LastName
    {
        get => _lastName;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Прізвище не може бути порожнім.", nameof(LastName));
            }

            if (value.Length > 50)
            {
                throw new ArgumentException("Прізвище не може бути довшим за 50 символів.", nameof(LastName));
            }

            _lastName = value;
        }
    }

    public DateTime DateOfBirth
    {
        get => _dateOfBirth;
        set
        {
            if (value > DateTime.Today)
            {
                throw new ArgumentOutOfRangeException(nameof(DateOfBirth), value, "Дата народження не може бути в майбутньому.");
            }

            if (value.Year < 1900)
            {
                throw new ArgumentOutOfRangeException(nameof(DateOfBirth), value, "Дата народження не може бути раніше 1900 року.");
            }

            _dateOfBirth = value;
        }
    }

    public BloodType BloodType { get; set; }

    public string Phone
    {
        get => _phone;
        set
        {
            if (value == null || value.Length != 10)
            {
                throw new ArgumentException("Телефон має містити рівно 10 цифр.", nameof(Phone));
            }

            foreach (char c in value)
            {
                if (c < '0' || c > '9')
                {
                    throw new ArgumentException("Телефон має містити лише цифри.", nameof(Phone));
                }
            }

            _phone = value;
        }
    }

    public string Email { get; set; } = string.Empty;
    
    public string FullName => $"{FirstName} {LastName}";

    public int Age
    {
        get
        {
            var today = DateTime.Today;
            int age = today.Year - DateOfBirth.Year;
            
            if (DateOfBirth.AddYears(age) > today)
            {
                age--;
            }

            return age;
        }
    }
    
    public bool IsAdult => Age >= 18;

    public Patient(string firstName, string lastName, DateTime dateOfBirth, BloodType bloodType, string phone)
    {
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
        BloodType = bloodType;
        Phone = phone;
        Email = "";
        Id = _nextId++;
    } 
    
    public Patient(string firstName, string lastName) 
        : this(firstName, lastName, new(2000, 1, 1), BloodType.Unknown, "0000000000")
    {
    }
    public Patient()
        : this("Невідомий", "Пацієнт")
    {
    }
    
    public string GetAgeCategory()
    {
        if (Age < 18)
            return "дитина";
        if (Age < 60)
            return "дорослий";
        
        return "літній";
    }
    
    public override string ToString()
    {
        return $"[{Id}] {FullName} | Вік: {ClinicFormatter.FormatAge(Age)} ({GetAgeCategory()}) | Кров: {ClinicFormatter.FormatBloodType(BloodType)} | Тел: {ClinicFormatter.FormatPhone(Phone)}";
    }
    
}