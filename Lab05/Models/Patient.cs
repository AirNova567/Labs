using ClinicApp.Enums;
using ClinicApp.Utils;

namespace ClinicApp.Models;

public class Patient
{
    private static int _nextId = 1;
    public int Id { get; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime  DateOfBirth { get; set; }
    public BloodType BloodType { get; set; }
    public string Phone { get; set; }
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
        Id = _nextId++;
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
        BloodType = bloodType;
        Phone = phone;
        Email = "";
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