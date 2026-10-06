using System;

namespace Lab03
{
    class Program
    {
        static void Main(string[] args)
        {
            Clinic clinic = new Clinic("Медична Клініка");

            clinic.Patients.Add(new Patient("Іван", "Петренко", new DateTime(1985, 5, 12), "A+", "0501234567"));
            clinic.Patients.Add(new Patient("Олена", "Коваль", new DateTime(1993, 8, 20), "B-", "0672345678"));
            clinic.Patients.Add(new Patient("Максим", "Бойко", new DateTime(2010, 3, 15), "O+", "0933456789"));
            clinic.Patients.Add(new Patient("Марія", "Ткач", new DateTime(2000, 1, 1), "Невідомо", "0000000000"));

            clinic.Doctors.Add(new Doctor("Олег", "Сидоренко", "Кардіологія", "LIC-001", "0441234567", 8, 16));
            clinic.Doctors.Add(new Doctor("Наталія", "Мороз", "Неврологія", "LIC-002", "0442345678", 9, 18));
            clinic.Doctors.Add(new Doctor("Андрій", "Власенко", "Педіатрія", "LIC-003", "0443456789"));

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
                    case "0":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Невідома опція, спробуйте ще раз.");
                        break;
                }
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
                        Console.Write("Ім'я: ");
                        string name = Console.ReadLine()!;
                        Console.Write("Прізвище: ");
                        string surname = Console.ReadLine()!;
                        clinic.Patients.Add(new Patient(name, surname));
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
                        Console.Write("Ім'я: ");
                        string name = Console.ReadLine()!;
                        Console.Write("Прізвище: ");
                        string surname = Console.ReadLine()!;
                        Console.Write("Спеціалізація: ");
                        string spec = Console.ReadLine()!;
                        clinic.Doctors.Add(new Doctor(name, surname, spec));
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
                                Console.Write("Введіть дату та час (формат dd.MM.yyyy HH:mm): ");
                                if (DateTime.TryParse(Console.ReadLine(), out DateTime dt))
                                {
                                    clinic.Appointments.Book(pId, dId, dt);
                                }
                                else
                                {
                                    Console.WriteLine("Некоректний формат дати.");
                                }
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

        static void RunGrowableTest()
        {
            Console.WriteLine("=== Тест GrowablePatientManager ===");
            Console.WriteLine("Додаємо пацієнтів одного за одним...");
            
            GrowablePatientManager growableManager = new GrowablePatientManager();

            for (int i = 1; i <= 20; i++)
            {
                growableManager.Add(new Patient($"Пацієнт{i}", $"Тестовий{i}", new DateTime(1990, 1, 1), "A+", "0000000000"));
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