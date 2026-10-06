using ClinicApp.Enums;

namespace ClinicApp.Models
{
    public class Appointment
    {
        private static int _nextId = 1;
        public int Id { get; }
        public int PatientId { get; }
        public int DoctorId { get; }

        public DateTime ScheduledAt { get; }
        public int DurationMinutes { get; }
        public AppointmentStatus Status { get; private set; } = AppointmentStatus.Scheduled;
        
        public string Notes { get; private set; } = string.Empty;

        public DateTime EndsAt => ScheduledAt.AddMinutes(DurationMinutes);

        public bool IsUpcoming => ScheduledAt > DateTime.Now && Status == AppointmentStatus.Scheduled;

        public Appointment(int patientId, int doctorId, DateTime scheduledAt, int durationMinutes = 30)
        {
            Id = _nextId++;
            PatientId = patientId;
            DoctorId = doctorId;
            ScheduledAt = scheduledAt;
            DurationMinutes = durationMinutes;
        }

        public bool Cancel(string reason = "")
        {
            if (Status != AppointmentStatus.Scheduled)
            {
                return false;
            }

            Status = AppointmentStatus.Cancelled;
            if (!string.IsNullOrEmpty(reason))
            {
                Notes = reason;
            }
            return true;
        }

        public bool Complete()
        {
            if (Status != AppointmentStatus.Scheduled)
            {
                return false;
            }

            Status = AppointmentStatus.Completed;
            return true;
        }

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