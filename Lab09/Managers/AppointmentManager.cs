using ClinicApp.Interfaces;
using ClinicApp.Models;

namespace ClinicApp.Managers;

public class AppointmentManager
{
    private const int MaxAppointments = 500;
    private readonly Appointment[] _appointments = new Appointment[MaxAppointments];
    private int _count = 0;
    
    private readonly PatientManager _patients;
    private readonly DoctorManager _doctors;

    public int Count => _count;
    
    public AppointmentManager(PatientManager patientManager, DoctorManager doctorManager)
    {
        _patients = patientManager;
        _doctors = doctorManager;
    }
    
    public Appointment? this[int index]
    {
        get
        {
            if (index < 0 || index >= _count)
            {
                return null;
            }

            return _appointments[index];
        }
    }
    
    private Appointment? FindById(int id)
    {
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].Id == id)
            {
                return _appointments[i];
            }
        }

        return null;
    }
    
    private bool CanBook(int patientId, int doctorId)
    {
        if (_patients.FindById(patientId) == null)
        {
            Console.WriteLine($"Помилка: пацієнта з ID {patientId} не знайдено.");
            return false;
        }

        if (_doctors.FindById(doctorId) == null)
        {
            Console.WriteLine($"Помилка: лікаря з ID {doctorId} не знайдено.");
            return false;
        }

        if (_count >= MaxAppointments)
        {
            Console.WriteLine("Помилка: досягнуто ліміту записів (500).");
            return false;
        }

        return true;
    }

    private void Store(Appointment appointment)
    {
        _appointments[_count++] = appointment;

        string patientName = _patients.FindById(appointment.PatientId)!.FullName;
        string doctorName = _doctors.FindById(appointment.DoctorId)!.FullName;
        Console.WriteLine($"Запис [{appointment.Id}] створено: {patientName} -> {doctorName} о {appointment.ScheduledAt:dd.MM.yyyy HH:mm}");
    }

    public bool Book(int patientId, int doctorId, DateTime scheduledAt, int durationMinutes = 30)
    {
        if (!CanBook(patientId, doctorId))
        {
            return false;
        }

        Store(new RegularAppointment(patientId, doctorId, scheduledAt, durationMinutes));
        return true;
    }

    public bool BookUrgent(int patientId, int doctorId, DateTime scheduledAt, string urgencyNote = "", int durationMinutes = 30)
    {
        if (!CanBook(patientId, doctorId))
        {
            return false;
        }

        Store(new UrgentAppointment(patientId, doctorId, scheduledAt, urgencyNote, durationMinutes));
        return true;
    }

    public bool BookSpecialist(int patientId, int doctorId, DateTime scheduledAt, string topic = "", int durationMinutes = 45)
    {
        if (!CanBook(patientId, doctorId))
        {
            return false;
        }

        Store(new SpecialistAppointment(patientId, doctorId, scheduledAt, topic, durationMinutes));
        return true;
    }

    public bool Cancel(int id, string reason = "")
    {
        var app = FindById(id);
        if (app != null)
        {
            return app.Cancel(reason);
        }

        return false;
    }
    
    public bool Complete(int id)
    {
        var app = FindById(id);
        if (app != null)
        {
            return app.Complete();
        }

        return false;
    }
    
    public static int CancelAll(ICancellable[] items, string reason = "")
    {
        int cancelled = 0;
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].Cancel(reason))
            {
                cancelled++;
            }
        }

        return cancelled;
    }

    public Appointment[] GetAll()
    {
        Appointment[] copy = new Appointment[_count];
        for (int i = 0; i < _count; i++)
        {
            copy[i] = _appointments[i];
        }

        return copy;
    }

    public Appointment[] GetByPatient(int patientId)
    {
        int matches = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].PatientId == patientId)
                matches++;
        }

        Appointment[] result = new Appointment[matches];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].PatientId == patientId)
                result[index++] = _appointments[i];
        }

        return result;
    }
    
    public Appointment[] GetByDoctor(int doctorId)
    {
        int matches = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].DoctorId == doctorId)
                matches++;
        }

        Appointment[] result = new Appointment[matches];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].DoctorId == doctorId)
                result[index++] = _appointments[i];
        }

        return result;
    }
    
    public Appointment[] GetByDate(DateTime date)
    {
        int matches = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].ScheduledAt.Date == date.Date)
                matches++;
        }

        Appointment[] result = new Appointment[matches];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].ScheduledAt.Date == date.Date)
                result[index++] = _appointments[i];
        }

        return result;
    }

    
    public Appointment[] GetByDate(int year, int month, int day)
    {
        return GetByDate(new DateTime(year, month, day));
    }

    public Appointment[] GetUpcoming()
    {
        int matches = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].IsUpcoming)
                matches++;
        }

        Appointment[] result = new Appointment[matches];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].IsUpcoming)
                result[index++] = _appointments[i];
        }

        return result;
    }
    
    public Appointment[] GetUrgent()
    {
        int matches = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i] is UrgentAppointment)
            {
                matches++;
            }
        }

        Appointment[] result = new Appointment[matches];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i] is UrgentAppointment)
            {
                result[index++] = _appointments[i];
            }
        }

        return result;
    }

    public Appointment[] GetSpecialist()
    {
        int matches = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i] is SpecialistAppointment)
            {
                matches++;
            }
        }

        Appointment[] result = new Appointment[matches];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i] is SpecialistAppointment)
            {
                result[index++] = _appointments[i];
            }
        }

        return result;
    }

    public Appointment[] GetRegular()
    {
        int matches = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i] is RegularAppointment)
            {
                matches++;
            }
        }

        Appointment[] result = new Appointment[matches];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i] is RegularAppointment)
            {
                result[index++] = _appointments[i];
            }
        }

        return result;
    }

    public void DisplayAppointment(Appointment appointment)
    {
        var patient = _patients.FindById(appointment.PatientId);
        string patientName = patient != null ? patient.FullName : $"Пацієнт #{appointment.PatientId}";

        var doctor = _doctors.FindById(appointment.DoctorId);
        string doctorName = doctor != null ? doctor.FullName : $"Лікар #{appointment.DoctorId}";

        string timeFormat = $"{appointment.ScheduledAt:dd.MM.yyyy HH:mm}–{appointment.EndsAt:HH:mm}";
        string output = $"[{appointment.Id}] {appointment.GetDescription()} | {patientName} → {doctorName} | {timeFormat} | {appointment.Status} | {appointment.GetCost().ToString("F2")} грн";

        if (appointment.Notes.Length > 0)
        {
            output += $" | {appointment.Notes}";
        }

        Console.WriteLine(output);
    }
    
    public void DisplayList(Appointment[] list)
    {
        if (list.Length == 0)
        {
            Console.WriteLine("не знайдено");
            return;
        }

        foreach (var app in list)
        {
            DisplayAppointment(app);
        }
    }
}