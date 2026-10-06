namespace Lab03;

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
    
    public bool Book(int patientId, int doctorId, DateTime scheduledAt, int durationMinutes = 30)
    {
        var patient = _patients.FindById(patientId);
        if (patient == null)
        {
            Console.WriteLine($"Помилка: пацієнта з ID {patientId} не знайдено.");
            return false;
        }

        var doctor = _doctors.FindById(doctorId);
        if (doctor == null)
        {
            Console.WriteLine($"Помилка: лікаря з ID {doctorId} не знайдено.");
            return false;
        }

        if (_count >= MaxAppointments)
        {
            Console.WriteLine("Помилка: досягнуто ліміту записів (500).");
            return false;
        }

        var appointment = new Appointment(patientId, doctorId, scheduledAt, durationMinutes);
        _appointments[_count++] = appointment;

        Console.WriteLine($"Запис [{appointment.Id}] створено: {patient.FullName} -> {doctor.FullName} о {scheduledAt:dd.MM.yyyy HH:mm}");
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
    
    public void DisplayAppointment(Appointment appointment)
    {
        var patient = _patients.FindById(appointment.PatientId);
        string patientName = patient != null ? patient.FullName : $"Пацієнт #{appointment.PatientId}";

        var doctor = _doctors.FindById(appointment.DoctorId);
        string doctorName = doctor != null ? doctor.FullName : $"Лікар #{appointment.DoctorId}";

        string timeFormat = $"{appointment.ScheduledAt:dd.MM.yyyy HH:mm}–{appointment.EndsAt:HH:mm}";
        string output = $"[{appointment.Id}] {patientName} -> {doctorName} | {timeFormat} | {appointment.Status}";

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