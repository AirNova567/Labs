namespace Lab03
{
    public class Appointment
    {
        private static int _nextId = 1;
        public int Id { get; }
        public int PatientId { get; }
        public int DoctorId { get; }

        public DateTime ScheduledAt { get; }
        public int DurationMinutes { get; }
        public string Status { get; private set; } = "Scheduled";
        
        // Примітка з приватним сетером
        public string Notes { get; private set; } = string.Empty;

        // Обчислювані властивості
        public DateTime EndsAt => ScheduledAt.AddMinutes(DurationMinutes);

        public bool IsUpcoming => ScheduledAt > DateTime.Now && Status == "Scheduled";

        // Конструктор (тривалість за замовчуванням 30 хвилин)
        public Appointment(int patientId, int doctorId, DateTime scheduledAt, int durationMinutes = 30)
        {
            Id = _nextId++;
            PatientId = patientId;
            DoctorId = doctorId;
            ScheduledAt = scheduledAt;
            DurationMinutes = durationMinutes;
        }

        // Метод скасування запису
        public bool Cancel(string reason = "")
        {
            if (Status != "Scheduled")
            {
                return false;
            }

            Status = "Cancelled";
            if (!string.IsNullOrEmpty(reason))
            {
                Notes = reason;
            }
            return true;
        }

        // Метод завершення запису
        public bool Complete()
        {
            if (Status != "Scheduled")
            {
                return false;
            }

            Status = "Completed";
            return true;
        }

        // Перевизначення ToString()
        public override string ToString()
        {
            string timeRange = $"{ScheduledAt:dd.MM.yyyy HH:mm}–{EndsAt:HH:mm}";
            string result = $"[{Id}] Пацієнт #{PatientId} → Лікар #{DoctorId} | {timeRange} | {Status}";

            if (!string.IsNullOrEmpty(Notes))
            {
                result += $" | {Notes}";
            }

            return result;
        }
    }
}