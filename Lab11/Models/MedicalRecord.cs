using ClinicApp.Utils;

namespace ClinicApp.Models;

public abstract class MedicalRecord
{
    private static int _nextId = 1;
    public int Id { get; }
    public int PatientId { get; }
    public int DoctorId { get; }
    public DateTime Date { get; }
    public string Notes { get; set; } = "";

    protected MedicalRecord(int patientId, int doctorId, DateTime date)
    {
        ClinicValidator.ValidatePositive(patientId, nameof(patientId));
        ClinicValidator.ValidatePositive(doctorId, nameof(doctorId));
        ClinicValidator.ValidateDate(date, nameof(date));

        PatientId = patientId;
        DoctorId = doctorId;
        Date = date;
        Id = _nextId++;
    }

    public abstract string GetSummary();

    public virtual string GetRecordType()
    {
        return "Медичний запис";
    }

    public virtual bool IsActive()
    {
        return Date.Date >= DateTime.Today.AddMonths(-6);
    }

    public override string ToString()
    {
        string result = $"[{Id}] {GetRecordType()} | {Date:dd.MM.yyyy} | {GetSummary()}";

        if (!string.IsNullOrEmpty(Notes))
        {
            result += " | " + Notes;
        }

        return result;
    }
}
