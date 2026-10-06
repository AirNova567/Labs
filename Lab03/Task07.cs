namespace Lab03;

public class Clinic
{
    public string Name { get; set; }
    
    public PatientManager Patients { get; }
    public DoctorManager Doctors { get; }
    public AppointmentManager Appointments { get; }

    public Clinic(string name)
    {
        Name = name;
        Patients = new PatientManager();
        Doctors = new DoctorManager();
        Appointments = new AppointmentManager(Patients, Doctors);
    }
    
    public void DisplaySchedule(DateTime date)
    {
        Console.WriteLine($"=== Розклад на {date:dd.MM.yyyy} ===");
        var schedule = Appointments.GetByDate(date);
        Appointments.DisplayList(schedule);
    }

 
    public void GenerateReport()
    {
        var upcoming = Appointments.GetUpcoming();
        var doctors = Doctors.GetAll();

        const int innerWidth = 58;

        Console.WriteLine("╔" + new string('═', innerWidth + 2) + "╗");
        PrintBoxLine($"Звіт — {Name}", innerWidth);
        Console.WriteLine("╠" + new string('═', innerWidth + 2) + "╣");
        PrintBoxLine($"Пацієнтів:        {Patients.Count}", innerWidth);
        PrintBoxLine($"Лікарів:          {Doctors.Count}", innerWidth);
        PrintBoxLine($"Майбутніх записів: {upcoming.Length}", innerWidth);
        Console.WriteLine("╠" + new string('═', innerWidth + 2) + "╣");
        PrintBoxLine("Навантаження лікарів (майбутні записи):", innerWidth);
        
        for (int i = 0; i < doctors.Length; i++)
        {
            int docId = doctors[i].Id;
            int count = 0;

            for (int j = 0; j < upcoming.Length; j++)
            {
                if (upcoming[j].DoctorId == docId)
                {
                    count++;
                }
            }

            PrintBoxLine($"  {doctors[i].FullName} ({doctors[i].Speciality}): {count} записів", innerWidth);
        }

        Console.WriteLine("╚" + new string('═', innerWidth + 2) + "╝");
    }
    
    private void PrintBoxLine(string text, int width)
    {
        Console.WriteLine($"║ {text.PadRight(width)} ║");
    }
}