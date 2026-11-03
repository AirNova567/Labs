using System.Globalization;
using ClinicApp.Enums;
using ClinicApp.Interfaces;
using ClinicApp.Managers;
using ClinicApp.Models;
using ClinicApp.Utils;

namespace ClinicApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

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

            SeedMedicalRecords(clinic);

            bool exit = false;
            while (!exit)
            {
                PrintMainMenu();
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
                        RunMedicalRecordsMenu(clinic);
                        break;
                    case "5":
                        RunBillingMenu(clinic);
                        break;
                    case "6":
                        clinic.GenerateReport();
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

        static void PrintMainMenu()
        {
            const int innerWidth = 46;
            string[] lines =
            {
                "           МЕДИЧНА КЛІНІКА",
                "  1. Пацієнти       — реєстрація, пошук",
                "  2. Лікарі         — персонал, розклад",
                "  3. Записи         — прийоми, скасування",
                "  4. Медична картка — діагнози, рецепти",
                "  5. Рахунки        — оплата, борги",
                "  6. Звіт           — загальна статистика",
                "  0. Вийти"
            };

            Console.WriteLine();
            Console.WriteLine("╔" + new string('═', innerWidth) + "╗");
            Console.WriteLine("║" + lines[0].PadRight(innerWidth) + "║");
            Console.WriteLine("╠" + new string('═', innerWidth) + "╣");
            for (int i = 1; i < lines.Length; i++)
            {
                Console.WriteLine("║" + lines[i].PadRight(innerWidth) + "║");
            }
            Console.WriteLine("╚" + new string('═', innerWidth) + "╝");
        }

        static void RunBillingMenu(Clinic clinic)
        {
            bool subExit = false;
            while (!subExit)
            {
                Console.WriteLine("\n── Рахунки ───────────────────");
                Console.WriteLine("  1. Борги пацієнта");
                Console.WriteLine("  2. Всі неоплачені записи");
                Console.WriteLine("  3. Оплатити запис");
                Console.WriteLine("  4. Загальний борг клініки");
                Console.WriteLine("  0. Назад");
                Console.Write("Оберіть опцію: ");

                string choice = Console.ReadLine()!;
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        if (TryReadInt("Введіть ID пацієнта: ", out int patientId))
                        {
                            IPayable[] unpaid = clinic.Billing.GetUnpaidByPatient(patientId);
                            clinic.Billing.DisplayUnpaid(unpaid);
                            Console.WriteLine($"Борг пацієнта: {clinic.Billing.GetPatientDebt(patientId).ToString("F2")} грн");
                        }
                        break;
                    case "2":
                        IPayable[] allUnpaid = clinic.Billing.GetAllUnpaid();
                        clinic.Billing.DisplayUnpaid(allUnpaid);
                        break;
                    case "3":
                        if (TryReadInt("Введіть ID запису: ", out int recordId))
                        {
                            if (clinic.Billing.PayAppointment(recordId))
                            {
                                Console.WriteLine($"Запис [{recordId}] оплачено.");
                            }
                            else
                            {
                                Console.WriteLine("Не вдалося оплатити: запис не знайдено, вже оплачено або скасовано.");
                            }
                        }
                        break;
                    case "4":
                        Console.WriteLine($"Загальний борг клініки: {clinic.Billing.GetTotalDebt().ToString("F2")} грн");
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

        static void ShowFreeHours(Clinic clinic)
        {
            if (!TryReadInt("Введіть ID лікаря: ", out int doctorId))
            {
                return;
            }

            Doctor? doctor = clinic.Doctors.FindById(doctorId);
            if (doctor == null)
            {
                Console.WriteLine("Лікаря не знайдено.");
                return;
            }

            ISchedulable schedulable = doctor;

            Console.Write("Введіть дату (формат dd.MM.yyyy): ");
            if (!TryParseDate(Console.ReadLine(), out DateTime date))
            {
                Console.WriteLine("Некоректний формат дати.");
                return;
            }

            if (!TryReadInt("Кількість годин: ", out int slotCount))
            {
                return;
            }

            try
            {
                DateTime[] slots = schedulable.GetAvailableSlots(date, slotCount);
                foreach (DateTime slot in slots)
                {
                    Console.WriteLine($"  {slot:HH:mm}");
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
            }
        }

        static void CancelPatientAppointments(Clinic clinic)
        {
            if (!TryReadPatientId(clinic, out int patientId))
            {
                return;
            }

            Console.Write("Причина скасування (Enter, щоб пропустити): ");
            string reason = Console.ReadLine()!;

            ICancellable[] items = clinic.Appointments.GetByPatient(patientId);
            int cancelled = AppointmentManager.CancelAll(items, reason);
            Console.WriteLine($"Скасовано записів: {cancelled}");
        }

        static bool TryParseDate(string? input, out DateTime result)
        {
            return DateTime.TryParseExact(input, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out result);
        }

        static bool TryParseDateTime(string? input, out DateTime result)
        {
            return DateTime.TryParseExact(input, "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out result);
        }

        static void SeedMedicalRecords(Clinic clinic)
        {
            DateTime today = DateTime.Today;

            clinic.MedicalRecords.Add(new Diagnosis(1, 1, today.AddDays(-30), "I10", "Гіпертонічна хвороба", true));
            clinic.MedicalRecords.Add(new Diagnosis(1, 3, today.AddDays(-5), "J06.9", "Гостра інфекція дихальних шляхів"));
            clinic.MedicalRecords.Add(new LabResult(1, 1, today.AddDays(-4), "Гемоглобін", 145, "г/л", "120–160", true));
            clinic.MedicalRecords.Add(new LabResult(1, 1, today.AddDays(-4), "Холестерин", 6.8, "ммоль/л", "3.0–5.2", false));
            clinic.MedicalRecords.Add(new Prescription(1, 1, today.AddDays(-5), "Лізиноприл", "10 мг", 30, "1 раз на добу вранці"));
            clinic.MedicalRecords.Add(new Prescription(2, 2, today.AddDays(-40), "Амоксицилін", "500 мг", 10));
            clinic.MedicalRecords.Add(new Diagnosis(2, 2, today.AddDays(-12), "G43.9", "Мігрень", true));
            clinic.MedicalRecords.Add(new LabResult(3, 3, today.AddDays(-2), "Глюкоза", 4.9, "ммоль/л", "3.9–5.6", true));
        }

        static bool TryReadInt(string prompt, out int value)
        {
            Console.Write(prompt);
            if (int.TryParse(Console.ReadLine(), out value))
            {
                return true;
            }

            Console.WriteLine("Некоректне число.");
            return false;
        }

        static bool TryReadPatientId(Clinic clinic, out int patientId)
        {
            if (!TryReadInt("Введіть ID пацієнта: ", out patientId))
            {
                return false;
            }

            if (clinic.Patients.FindById(patientId) == null)
            {
                Console.WriteLine("Пацієнта не знайдено.");
                return false;
            }

            return true;
        }

        static bool TryReadDoctorId(Clinic clinic, out int doctorId)
        {
            if (!TryReadInt("Введіть ID лікаря: ", out doctorId))
            {
                return false;
            }

            if (clinic.Doctors.FindById(doctorId) == null)
            {
                Console.WriteLine("Лікаря не знайдено.");
                return false;
            }

            return true;
        }

        static void AddDiagnosis(Clinic clinic)
        {
            if (!TryReadPatientId(clinic, out int patientId) || !TryReadDoctorId(clinic, out int doctorId))
            {
                return;
            }

            Console.Write("Код діагнозу (наприклад J06.9): ");
            string code = Console.ReadLine()!;
            Console.Write("Опис: ");
            string description = Console.ReadLine()!;
            Console.Write("Хронічне? (1/0): ");
            bool isChronic = Console.ReadLine() == "1";

            try
            {
                clinic.MedicalRecords.Add(new Diagnosis(patientId, doctorId, DateTime.Today, code, description, isChronic));
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
            }
        }

        static void AddLabResult(Clinic clinic)
        {
            if (!TryReadPatientId(clinic, out int patientId) || !TryReadDoctorId(clinic, out int doctorId))
            {
                return;
            }

            Console.Write("Назва аналізу: ");
            string testName = Console.ReadLine()!;
            Console.Write("Значення (число через крапку): ");
            if (!double.TryParse(Console.ReadLine(), NumberStyles.Float, CultureInfo.InvariantCulture, out double value))
            {
                Console.WriteLine("Некоректне число.");
                return;
            }

            Console.Write("Одиниця виміру: ");
            string unit = Console.ReadLine()!;
            Console.Write("Референсний діапазон (наприклад 4.0–9.0): ");
            string range = Console.ReadLine()!;
            Console.Write("В нормі? (1/0): ");
            bool isNormal = Console.ReadLine() == "1";

            try
            {
                clinic.MedicalRecords.Add(new LabResult(patientId, doctorId, DateTime.Today, testName, value, unit, range, isNormal));
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
            }
        }

        static void AddPrescription(Clinic clinic)
        {
            if (!TryReadPatientId(clinic, out int patientId) || !TryReadDoctorId(clinic, out int doctorId))
            {
                return;
            }

            Console.Write("Препарат: ");
            string medication = Console.ReadLine()!;
            Console.Write("Дозування (наприклад 10 мг): ");
            string dosage = Console.ReadLine()!;
            if (!TryReadInt("Кількість днів: ", out int days))
            {
                return;
            }

            Console.Write("Інструкції (Enter, щоб пропустити): ");
            string instructions = Console.ReadLine()!;

            try
            {
                clinic.MedicalRecords.Add(new Prescription(patientId, doctorId, DateTime.Today, medication, dosage, days, instructions));
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
            }
        }

        static void RunMedicalRecordsMenu(Clinic clinic)
        {
            bool subExit = false;
            while (!subExit)
            {
                Console.WriteLine("\n--- ПІДМЕНЮ: МЕДИЧНА КАРТКА ---");
                Console.WriteLine("1. Картка пацієнта");
                Console.WriteLine("2. Усі записи пацієнта");
                Console.WriteLine("3. Додати діагноз");
                Console.WriteLine("4. Додати результат аналізу");
                Console.WriteLine("5. Додати рецепт");
                Console.WriteLine("6. Записи лікаря");
                Console.WriteLine("0. Назад");
                Console.Write("Оберіть опцію: ");

                string choice = Console.ReadLine()!;
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        if (TryReadPatientId(clinic, out int cardPatientId))
                        {
                            clinic.MedicalRecords.DisplayPatientSummary(cardPatientId);
                        }
                        break;
                    case "2":
                        if (TryReadPatientId(clinic, out int recordsPatientId))
                        {
                            MedicalRecord[] records = clinic.MedicalRecords.GetByPatient(recordsPatientId);
                            if (records.Length == 0)
                            {
                                Console.WriteLine("не знайдено");
                            }

                            foreach (MedicalRecord record in records)
                            {
                                Console.WriteLine($"{record.GetRecordType(),-8} → {record}");
                            }
                        }
                        break;
                    case "3":
                        AddDiagnosis(clinic);
                        break;
                    case "4":
                        AddLabResult(clinic);
                        break;
                    case "5":
                        AddPrescription(clinic);
                        break;
                    case "6":
                        if (TryReadDoctorId(clinic, out int doctorId))
                        {
                            clinic.MedicalRecords.DisplayList(clinic.MedicalRecords.GetByDoctor(doctorId));
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

        static void AddPatient(Clinic clinic)
        {
            try
            {
                Console.Write("Ім'я: ");
                string name = Console.ReadLine()!;
                Console.Write("Прізвище: ");
                string surname = Console.ReadLine()!;
                Console.Write("Дата народження (dd.MM.yyyy): ");
                if (!TryParseDate(Console.ReadLine(), out DateTime birthDate))
                {
                    Console.WriteLine("Некоректний формат дати.");
                    return;
                }

                Console.Write("Телефон (10 цифр): ");
                string phone = Console.ReadLine()!;

                Console.Write("Email (можна пропустити): ");
                string email = Console.ReadLine()!;

                ClinicValidator.ValidateEmail(email);

                Patient patient = new Patient(name, surname, birthDate, BloodType.Unknown, phone);
                patient.Email = email;
                clinic.Patients.Add(patient);
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
                if (!TryParseDateTime(Console.ReadLine(), out DateTime dt))
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
                Console.WriteLine("7. Запустити тест GrowablePatientManager (Завдання 8)");
                Console.WriteLine("8. Демонстрація індексаторів, перевантажень та out (Lab04)");
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
                    case "7":
                        RunGrowableTest();
                        break;
                    case "8":
                        RunMembersDemo(clinic);
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
                Console.WriteLine("5. Вільні години лікаря");
                Console.WriteLine("6. Видалити за ID");
                Console.WriteLine("7. Статистика лікарів");
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
                        ShowFreeHours(clinic);
                        break;
                    case "6":
                        Console.Write("Введіть ID для видалення: ");
                        if (int.TryParse(Console.ReadLine(), out int removeId))
                        {
                            bool success = clinic.Doctors.Remove(removeId);
                            Console.WriteLine(success ? "Лікаря видалено." : "Лікаря з таким ID не знайдено.");
                        }
                        break;
                    case "7":
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
                Console.WriteLine("6. Розклад на дату");
                Console.WriteLine("7. Записи лікаря");
                Console.WriteLine("8. Скасувати всі записи пацієнта");
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
                    case "6":
                        Console.Write("Введіть дату (формат dd.MM.yyyy): ");
                        if (TryParseDate(Console.ReadLine(), out DateTime scheduleDate))
                        {
                            clinic.DisplaySchedule(scheduleDate);
                        }
                        else
                        {
                            Console.WriteLine("Некоректний формат дати.");
                        }
                        break;
                    case "7":
                        if (TryReadDoctorId(clinic, out int scheduleDoctorId))
                        {
                            clinic.Appointments.DisplayList(clinic.Appointments.GetByDoctor(scheduleDoctorId));
                        }
                        break;
                    case "8":
                        CancelPatientAppointments(clinic);
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

        static void RunMembersDemo(Clinic clinic)
        {
            Console.WriteLine("=== Демонстрація індексаторів, перевантажень та out ===");

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