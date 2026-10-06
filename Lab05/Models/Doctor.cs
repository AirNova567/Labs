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
            ClinicValidator.ValidateName(value, nameof(FirstName));

            _firstName = value;
        }
    }

    public string LastName
    {
        get => _lastName;
        set
        {
            ClinicValidator.ValidateName(value, nameof(LastName));

            _lastName = value;
        }
    }

    public Speciality Speciality { get; set; }

    public string LicenseNumber
    {
        get => _licenseNumber;
        set
        {
            ClinicValidator.ValidateName(value, nameof(LicenseNumber));

            _licenseNumber = value;
        }
    }

    public string Phone
    {
        get => _phone;
        set
        {
            ClinicValidator.ValidatePhone(value);

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