using ClinicApp.Enums;
using ClinicApp.Managers;
using ClinicApp.Models;
using ClinicApp.Utils;

namespace ClinicApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Clinic clinic = new Clinic("Медична Клініка");

            clinic.Patients.Add(new Patient("Іван", "Петренко", new DateTime(1985, 5, 12), BloodType.APositive, "0501234567"));
            clinic.Patients.Add(new Patient("Олена", "Коваль", new DateTime(1993, 8, 20), BloodType.BNegative, "0672345678"));
            clinic.Patients.Add(new Patient("Максим", "Бойко", new DateTime(2010, 3, 15), BloodType.OPositive, "0933456789"));
            clinic.Patients.Add(new Patient("Марія", "Ткач", new DateTime(2000, 1, 1), BloodType.Unknown, "0000000000"));

            clinic.Doctors.Add(new Doctor("Олег", "Сидоренко", Speciality.Cardiology, "LIC-001", "0441234567", new WorkSchedule(8, 16)));
            clinic.Doctors.Add(new Doctor("Наталія", "Мороз", Speciality.Neurology, "LIC-002", "0442345678", new WorkSchedule(9, 18)));
            clinic.Doctors.Add(new Doctor("Андрій", "Власенко", Speciality.Pediatrics, "LIC-003", "0443456789"));

            clinic.Appointments.Book(1, 1, new DateTime(2026, 5, 9, 10, 0, 0), 30);
            clinic.Appointments.Book(2, 2, new DateTime(2026, 5, 9, 11, 0, 0), 45);
            clinic.Appointments.Book(3, 3, new DateTime(2026, 5, 10, 9, 0, 0), 20);

            bool exit = false;
            while (!exit)
            {
                Console.WriteLine($"\n--- ГОЛОВНЕ МЕНЮ: {clinic.Name.ToUpper()} ---");
                Console.WriteLine("1. Підменю: Пацієнти");
                Console.WriteLine("2. Підменю: Лікарі");
                Console.WriteLine("3. Підменю: Записи на прийом");
                Console.WriteLine("4. Показати розклад на дату");
                Console.WriteLine("5. Згенерувати звіт клініки");
                Console.WriteLine("6. Запустити тест GrowablePatientManager (Завдання 8)");
                Console.WriteLine("7. Демонстрація Lab05 (індексатор, перевантаження, out, ?., ??)");
                Console.WriteLine("0. Вихід");
                Console.Write("Оберіть опцію: ");

                string choice = Console.ReadLine()!;
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        RunPatientsMenu(clinic);
                        break;
                    case "2":
                        RunDoctorsMenu(clinic);
                        break;
                    case "3":
                        RunAppointmentsMenu(clinic);
                        break;
                    case "4":
                        Console.Write("Введіть дату (формат dd.MM.yyyy): ");
                        if (DateTime.TryParse(Console.ReadLine(), out DateTime scheduleDate))
                        {
                            clinic.DisplaySchedule(scheduleDate);
                        }
                        else
                        {
                            Console.WriteLine("Некоректний формат дати.");
                        }
                        break;
                    case "5":
                        clinic.GenerateReport();
                        break;
                    case "6":
                        RunGrowableTest();
                        break;
                    case "7":
                        RunLab05Demo(clinic);
                        break;
                    case "0":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Невідома опція, спробуйте ще раз.");
                        break;
                }
            }
        }

        static void AddPatient(Clinic clinic)
        {
            try
            {
                Console.Write("Ім'я: ");
                string name = Console.ReadLine()!;
                Console.Write("Прізвище: ");
                string surname = Console.ReadLine()!;
                Console.Write("Дата народження (dd.MM.yyyy): ");
                if (!DateTime.TryParse(Console.ReadLine(), out DateTime birthDate))
                {
                    Console.WriteLine("Некоректний формат дати.");
                    return;
                }

                Console.Write("Телефон (10 цифр): ");
                string phone = Console.ReadLine()!;

                clinic.Patients.Add(new Patient(name, surname, birthDate, BloodType.Unknown, phone));
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
            }
        }

        static void AddDoctor(Clinic clinic)
        {
            try
            {
                Console.Write("Ім'я: ");
                string name = Console.ReadLine()!;
                Console.Write("Прізвище: ");
                string surname = Console.ReadLine()!;
                Console.WriteLine("Спеціалізації: " + string.Join(", ", Enum.GetNames<Speciality>()));
                Console.Write("Спеціалізація: ");
                if (!Enum.TryParse(Console.ReadLine(), true, out Speciality spec))
                {
                    spec = Speciality.General;
                }

                Console.Write("Номер ліцензії: ");
                string license = Console.ReadLine()!;
                Console.Write("Телефон (10 цифр): ");
                string phone = Console.ReadLine()!;

                Console.Write("Початок робочого дня (година): ");
                if (!int.TryParse(Console.ReadLine(), out int start))
                {
                    Console.WriteLine("Некоректне число.");
                    return;
                }

                Console.Write("Кінець робочого дня (година): ");
                if (!int.TryParse(Console.ReadLine(), out int end))
                {
                    Console.WriteLine("Некоректне число.");
                    return;
                }

                clinic.Doctors.Add(new Doctor(name, surname, spec, license, phone, new WorkSchedule(start, end)));
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
            }
        }

        static void BookAppointment(Clinic clinic, int patientId, int doctorId)
        {
            try
            {
                Console.Write("Введіть дату та час (формат dd.MM.yyyy HH:mm): ");
                if (!DateTime.TryParse(Console.ReadLine(), out DateTime dt))
                {
                    Console.WriteLine("Некоректний формат дати.");
                    return;
                }

                Console.Write("Тривалість у хвилинах (за замовчуванням 30): ");
                string durationInput = Console.ReadLine()!;
                int duration = 30;
                if (!string.IsNullOrWhiteSpace(durationInput) && !int.TryParse(durationInput, out duration))
                {
                    Console.WriteLine("Некоректне число.");
                    return;
                }

                clinic.Appointments.Book(patientId, doctorId, dt, duration);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
            }
        }

        static void RunPatientsMenu(Clinic clinic)
        {
            bool subExit = false;
            while (!subExit)
            {
                Console.WriteLine("\n--- ПІДМЕНЮ: ПАЦІЄНТИ ---");
                Console.WriteLine("1. Показати всіх");
                Console.WriteLine("2. Додати пацієнта");
                Console.WriteLine("3. Знайти за ID");
                Console.WriteLine("4. Знайти за ім'ям");
                Console.WriteLine("5. Видалити за ID");
                Console.WriteLine("6. Статистика");
                Console.WriteLine("0. Назад");
                Console.Write("Оберіть опцію: ");

                string choice = Console.ReadLine()!;
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        clinic.Patients.DisplayAll();
                        break;
                    case "2":
                        AddPatient(clinic);
                        break;
                    case "3":
                        Console.Write("Введіть ID: ");
                        if (int.TryParse(Console.ReadLine(), out int id))
                        {
                            Patient? found = clinic.Patients.FindById(id);
                            Console.WriteLine(found != null ? found.ToString() : "Пацієнта не знайдено.");
                        }
                        break;
                    case "4":
                        Console.Write("Введіть ім'я або прізвище для пошуку: ");
                        string query = Console.ReadLine()!;
                        Patient[] matches = clinic.Patients.FindByName(query);
                        if (matches.Length == 0)
                        {
                            Console.WriteLine("Нічого не знайдено.");
                        }
                        else
                        {
                            foreach (var p in matches)
                            {
                                Console.WriteLine(p);
                            }
                        }
                        break;
                    case "5":
                        Console.Write("Введіть ID для видалення: ");
                        if (int.TryParse(Console.ReadLine(), out int removeId))
                        {
                            bool success = clinic.Patients.Remove(removeId);
                            Console.WriteLine(success ? "Пацієнта видалено." : "Пацієнта з таким ID не знайдено.");
                        }
                        break;
                    case "6":
                        clinic.Patients.DisplayStats();
                        break;
                    case "0":
                        subExit = true;
                        break;
                    default:
                        Console.WriteLine("Невідома опція.");
                        break;
                }
            }
        }

        static void RunDoctorsMenu(Clinic clinic)
        {
            bool subExit = false;
            while (!subExit)
            {
                Console.WriteLine("\n--- ПІДМЕНЮ: ЛІКАРІ ---");
                Console.WriteLine("1. Показати всіх лікарів");
                Console.WriteLine("2. Додати лікаря");
                Console.WriteLine("3. Знайти за ID");
                Console.WriteLine("4. Знайти за спеціальністю");
                Console.WriteLine("5. Видалити за ID");
                Console.WriteLine("6. Статистика лікарів");
                Console.WriteLine("0. Назад");
                Console.Write("Оберіть опцію: ");

                string choice = Console.ReadLine()!;
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        clinic.Doctors.DisplayAll();
                        break;
                    case "2":
                        AddDoctor(clinic);
                        break;
                    case "3":
                        Console.Write("Введіть ID лікаря: ");
                        if (int.TryParse(Console.ReadLine(), out int id))
                        {
                            Doctor? found = clinic.Doctors.FindById(id);
                            Console.WriteLine(found != null ? found.ToString() : "Лікаря не знайдено.");
                        }
                        break;
                    case "4":
                        Console.Write("Введіть спеціальність для пошуку: ");
                        string query = Console.ReadLine()!;
                        Doctor[] matches = clinic.Doctors.FindBySpeciality(query);
                        if (matches.Length == 0)
                        {
                            Console.WriteLine("Нічого не знайдено.");
                        }
                        else
                        {
                            foreach (var d in matches)
                            {
                                Console.WriteLine(d);
                            }
                        }
                        break;
                    case "5":
                        Console.Write("Введіть ID для видалення: ");
                        if (int.TryParse(Console.ReadLine(), out int removeId))
                        {
                            bool success = clinic.Doctors.Remove(removeId);
                            Console.WriteLine(success ? "Лікаря видалено." : "Лікаря з таким ID не знайдено.");
                        }
                        break;
                    case "6":
                        clinic.Doctors.DisplayStats();
                        break;
                    case "0":
                        subExit = true;
                        break;
                    default:
                        Console.WriteLine("Невідома опція.");
                        break;
                }
            }
        }

        static void RunAppointmentsMenu(Clinic clinic)
        {
            bool subExit = false;
            while (!subExit)
            {
                Console.WriteLine("\n--- ПІДМЕНЮ: ЗАПИСИ ---");
                Console.WriteLine("1. Показати майбутні записи");
                Console.WriteLine("2. Створити запис (Book)");
                Console.WriteLine("3. Скасувати запис (Cancel)");
                Console.WriteLine("4. Завершити запис (Complete)");
                Console.WriteLine("5. Записи конкретного пацієнта");
                Console.WriteLine("0. Повернутися назад");
                Console.Write("Оберіть опцію: ");

                string subChoice = Console.ReadLine()!;
                Console.WriteLine();

                switch (subChoice)
                {
                    case "1":
                        Console.WriteLine("Майбутні записи:");
                        clinic.Appointments.DisplayList(clinic.Appointments.GetUpcoming());
                        break;
                    case "2":
                        clinic.Patients.DisplayAll();
                        clinic.Doctors.DisplayAll();
                        Console.Write("Введіть ID пацієнта: ");
                        if (int.TryParse(Console.ReadLine(), out int pId))
                        {
                            Console.Write("Введіть ID лікаря: ");
                            if (int.TryParse(Console.ReadLine(), out int dId))
                            {
                                BookAppointment(clinic, pId, dId);
                            }
                        }
                        break;
                    case "3":
                        Console.Write("Введіть ID запису для скасування: ");
                        if (int.TryParse(Console.ReadLine(), out int cancelId))
                        {
                            Console.Write("Причина скасування (можна пропустити): ");
                            string reason = Console.ReadLine()!;
                            clinic.Appointments.Cancel(cancelId, reason);
                        }
                        break;
                    case "4":
                        Console.Write("Введіть ID запису для завершення: ");
                        if (int.TryParse(Console.ReadLine(), out int compId))
                        {
                            clinic.Appointments.Complete(compId);
                        }
                        break;
                    case "5":
                        Console.Write("Введіть ID пацієнта: ");
                        if (int.TryParse(Console.ReadLine(), out int patientIdForSearch))
                        {
                            clinic.Appointments.DisplayList(clinic.Appointments.GetByPatient(patientIdForSearch));
                        }
                        break;
                    case "0":
                        subExit = true;
                        break;
                    default:
                        Console.WriteLine("Невідома опція.");
                        break;
                }
            }
        }

        static void RunLab05Demo(Clinic clinic)
        {
            Console.WriteLine("=== Демонстрація Lab05 ===");

            Console.WriteLine($"Patients[0]   → {clinic.Patients[0]?.FullName ?? "не знайдено"}");
            Console.WriteLine($"Patients[999] → {clinic.Patients[999]?.FullName ?? "не знайдено"}");
            Console.WriteLine($"Doctors[1]    → {clinic.Doctors[1]?.FullName ?? "не знайдено"}");
            Console.WriteLine($"Appointments[0] → {clinic.Appointments[0]?.ToString() ?? "не знайдено"}");

            Doctor[] cardiologists = clinic.Doctors.FindBySpeciality(Speciality.Cardiology);
            Doctor[] found = clinic.Doctors.FindBySpeciality("кардіо");
            Console.WriteLine($"FindBySpeciality(Speciality.Cardiology) → {cardiologists.Length}");
            Console.WriteLine($"FindBySpeciality(\"кардіо\")              → {found.Length}");

            Appointment[] byDate = clinic.Appointments.GetByDate(2026, 5, 9);
            Appointment[] byDateTime = clinic.Appointments.GetByDate(new DateTime(2026, 5, 9));
            Console.WriteLine($"GetByDate(2026, 5, 9) → {byDate.Length}, GetByDate(DateTime) → {byDateTime.Length}");

            if (clinic.Patients.TryFindById(3, out Patient patient))
            {
                Console.WriteLine($"TryFindById(3)  → {patient.FullName}");
            }

            if (!clinic.Patients.TryFindById(99, out Patient _))
            {
                Console.WriteLine("TryFindById(99) → false");
            }

            if (!clinic.Doctors.TryFindById(99, out Doctor _))
            {
                Console.WriteLine("Doctors.TryFindById(99) → false");
            }

            string name = clinic.Patients.FindById(99)?.FullName ?? "не знайдено";
            Console.WriteLine($"FindById(99)?.FullName ?? ... → {name}");

            Patient[] bNegative = clinic.Patients.FindByBloodType(BloodType.BNegative);
            Console.WriteLine($"FindByBloodType(BNegative) → {bNegative.Length}");
            foreach (Patient p in bNegative)
            {
                Console.WriteLine($"  {p}");
            }

            WorkSchedule morning = new(8, 16);
            Console.WriteLine($"WorkSchedule → {morning}");
            Console.WriteLine($"FormatAge: {ClinicFormatter.FormatAge(1)}, {ClinicFormatter.FormatAge(3)}, {ClinicFormatter.FormatAge(11)}, {ClinicFormatter.FormatAge(21)}");
        }

        static void RunGrowableTest()
        {
            Console.WriteLine("=== Тест GrowablePatientManager ===");
            Console.WriteLine("Додаємо пацієнтів одного за одним...");
            
            GrowablePatientManager growableManager = new GrowablePatientManager();

            for (int i = 1; i <= 20; i++)
            {
                growableManager.Add(new Patient($"Пацієнт{i}", $"Тестовий{i}", new DateTime(1990, 1, 1), BloodType.APositive, "0000000000"));
            }

            Console.WriteLine("\nТест пошуку:");
            Patient? found10 = growableManager.FindById(10);
            Console.WriteLine($"  FindById(10) → {(found10 != null ? found10.FullName : "не знайдено")}");

            Patient? found99 = growableManager.FindById(99);
            Console.WriteLine($"  FindById(99) → {(found99 != null ? found99.FullName : "не знайдено")}");

            Console.WriteLine("\nПорівняння:");
            Console.WriteLine("  PatientManager:         100 місць (фіксовано)");
            Console.WriteLine($"  GrowablePatientManager:  {growableManager.Capacity} місця (зросте при потребі)");
            Console.WriteLine("\nНатисніть Enter, щоб повернутися до головного меню...");
            Console.ReadLine();
        }
    }
}