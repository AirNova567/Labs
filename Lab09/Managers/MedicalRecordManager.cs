using ClinicApp.Models;

namespace ClinicApp.Managers;

public class MedicalRecordManager
{
    private const int MaxRecords = 1000;
    private readonly MedicalRecord[] _records = new MedicalRecord[MaxRecords];
    private int _count = 0;

    public int Count => _count;

    public MedicalRecord? this[int index]
    {
        get
        {
            if (index < 0 || index >= _count)
            {
                return null;
            }

            return _records[index];
        }
    }

    public void Add(MedicalRecord record)
    {
        if (_count >= MaxRecords)
        {
            Console.WriteLine("Помилка: досягнуто ліміту записів (1000).");
            return;
        }

        _records[_count++] = record;
        Console.WriteLine($"Запис [{record.Id}] {record.GetRecordType()} додано.");
    }

    public MedicalRecord? FindById(int id)
    {
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].Id == id)
            {
                return _records[i];
            }
        }

        return null;
    }

    public MedicalRecord[] GetByPatient(int patientId)
    {
        int matches = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId)
            {
                matches++;
            }
        }

        MedicalRecord[] result = new MedicalRecord[matches];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId)
            {
                result[index++] = _records[i];
            }
        }

        return result;
    }

    public MedicalRecord[] GetByDoctor(int doctorId)
    {
        int matches = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].DoctorId == doctorId)
            {
                matches++;
            }
        }

        MedicalRecord[] result = new MedicalRecord[matches];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].DoctorId == doctorId)
            {
                result[index++] = _records[i];
            }
        }

        return result;
    }

    public Diagnosis[] GetDiagnoses(int patientId)
    {
        int matches = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId && _records[i] is Diagnosis)
            {
                matches++;
            }
        }

        Diagnosis[] result = new Diagnosis[matches];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId && _records[i] is Diagnosis item)
            {
                result[index++] = item;
            }
        }

        return result;
    }

    public LabResult[] GetLabResults(int patientId)
    {
        int matches = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId && _records[i] is LabResult)
            {
                matches++;
            }
        }

        LabResult[] result = new LabResult[matches];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId && _records[i] is LabResult item)
            {
                result[index++] = item;
            }
        }

        return result;
    }

    public Prescription[] GetPrescriptions(int patientId)
    {
        int matches = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId && _records[i] is Prescription)
            {
                matches++;
            }
        }

        Prescription[] result = new Prescription[matches];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId && _records[i] is Prescription item)
            {
                result[index++] = item;
            }
        }

        return result;
    }

    public Diagnosis[] GetChronicDiagnoses(int patientId)
    {
        int matches = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId && _records[i] is Diagnosis chronic && chronic.IsChronic)
            {
                matches++;
            }
        }

        Diagnosis[] result = new Diagnosis[matches];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId && _records[i] is Diagnosis item && item.IsChronic)
            {
                result[index++] = item;
            }
        }

        return result;
    }

    public Prescription[] GetActivePrescriptions(int patientId)
    {
        int matches = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId && _records[i] is Prescription current && current.IsActive())
            {
                matches++;
            }
        }

        Prescription[] result = new Prescription[matches];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId && _records[i] is Prescription item && item.IsActive())
            {
                result[index++] = item;
            }
        }

        return result;
    }

    public void DisplayPatientSummary(int patientId)
    {
        Diagnosis[] diagnoses = GetDiagnoses(patientId);
        LabResult[] labResults = GetLabResults(patientId);
        Prescription[] prescriptions = GetPrescriptions(patientId);
        Diagnosis[] chronic = GetChronicDiagnoses(patientId);
        Prescription[] active = GetActivePrescriptions(patientId);

        Console.WriteLine($"=== Медична картка пацієнта #{patientId} ===");
        Console.WriteLine($"Діагнозів:  {diagnoses.Length}");
        Console.WriteLine($"Аналізів:   {labResults.Length}");
        Console.WriteLine($"Рецептів:   {prescriptions.Length}");

        if (chronic.Length > 0)
        {
            Console.WriteLine("Хронічні діагнози:");
            foreach (Diagnosis diagnosis in chronic)
            {
                Console.WriteLine($"  {diagnosis.GetSummary()}");
            }
        }

        if (active.Length > 0)
        {
            Console.WriteLine("Активні рецепти:");
            foreach (Prescription prescription in active)
            {
                Console.WriteLine($"  {prescription.GetSummary()} — до {prescription.ExpiresAt:dd.MM.yyyy}");
            }
        }

        Console.WriteLine("============================");
    }

    public void DisplayAll()
    {
        if (_count == 0)
        {
            Console.WriteLine("Список записів порожній.");
            return;
        }

        Console.WriteLine($"=== Медичні записи ({_count} / {MaxRecords}) ===");
        for (int i = 0; i < _count; i++)
        {
            Console.WriteLine(_records[i]);
        }
        Console.WriteLine(new string('-', 50));
    }

    public void DisplayList(MedicalRecord[] list)
    {
        if (list.Length == 0)
        {
            Console.WriteLine("не знайдено");
            return;
        }

        foreach (MedicalRecord record in list)
        {
            Console.WriteLine(record);
        }
    }
}
