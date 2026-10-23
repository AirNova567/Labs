using ClinicApp.Enums;
using ClinicApp.Interfaces;
using ClinicApp.Utils;

namespace ClinicApp.Models
{
    public class Appointment : IPayable
    {
        private static int _nextId = 1;
        public int Id { get; }
        public int PatientId { get; }
        public int DoctorId { get; }

        public DateTime ScheduledAt { get; }
        private int _durationMinutes;
        private bool _isPaid;

        public int DurationMinutes
        {
            get => _durationMinutes;
            set
            {
                ClinicValidator.ValidatePositive(value, nameof(DurationMinutes));

                _durationMinutes = value;
            }
        }
        public AppointmentStatus Status { get; private set; } = AppointmentStatus.Scheduled;
        
        public string Notes { get; private set; } = string.Empty;

        public DateTime EndsAt => ScheduledAt.AddMinutes(DurationMinutes);

        public bool IsUpcoming => ScheduledAt > DateTime.Now && Status == AppointmentStatus.Scheduled;

        public Appointment(int patientId, int doctorId, DateTime scheduledAt, int durationMinutes = 30)
        {
            PatientId = patientId;
            DoctorId = doctorId;
            ScheduledAt = scheduledAt;
            DurationMinutes = durationMinutes;
            Id = _nextId++;
        }

        public bool IsPaid => _isPaid;

        public decimal GetCost()
        {
            return DurationMinutes * 10m;
        }

        public void MarkPaid()
        {
            if (Status == AppointmentStatus.Cancelled)
            {
                return;
            }

            _isPaid = true;
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