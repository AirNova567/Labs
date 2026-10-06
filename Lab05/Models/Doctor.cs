using ClinicApp.Enums;
using ClinicApp.Utils;

namespace ClinicApp.Models;

public class Doctor
{
    private static int _nextId = 1;
    public int Id { get; }

    private string _firstName = "";
    private string _lastName = "";
    private string _licenseNumber = "";
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

    public Speciality Speciality { get; set; }

    public string LicenseNumber
    {
        get => _licenseNumber;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Номер ліцензії не може бути порожнім.", nameof(LicenseNumber));
            }

            if (value.Length > 50)
            {
                throw new ArgumentException("Номер ліцензії не може бути довшим за 50 символів.", nameof(LicenseNumber));
            }

            _licenseNumber = value;
        }
    }

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

    public WorkSchedule Schedule { get; set; }
    public string FullName => $"{FirstName} {LastName}";
    
    public bool IsAvailableNow => Schedule.IsNow;
    
    public Doctor(string firstName, string lastName, Speciality speciality, string licenseNumber, string phone, WorkSchedule schedule)
    {
        FirstName = firstName;
        LastName = lastName;
        Speciality = speciality;
        LicenseNumber = licenseNumber;
        Phone = phone;
        Schedule = schedule;
        Id = _nextId++;
    }
    
    public Doctor(string firstName, string lastName, Speciality speciality, string licenseNumber, string phone)
        : this(firstName, lastName, speciality, licenseNumber, phone, new WorkSchedule(8, 17))
    {
    }
    
    public Doctor(string firstName, string lastName, Speciality speciality)
        : this(firstName, lastName, speciality, "LIC-000", "0000000000")
    {
    }
    
    public Doctor()
        : this("Невідомий", "Лікар", Speciality.General)
    {
    }
    
    public bool CanAcceptAt(int hour)
    {
        return Schedule.Contains(hour);
    }
    
    public override string ToString()
    {
        string status = IsAvailableNow ? "доступний" : "не в робочий час";
        return $"[{Id}] {FullName} | {ClinicFormatter.FormatSpeciality(Speciality)} | {LicenseNumber} | Тел: {ClinicFormatter.FormatPhone(Phone)} | {Schedule} | {status}";
    }
}