using ClinicApp.Models;

namespace ClinicApp.Managers;

public class AnalyticsManager
{
    private readonly AppointmentManager _appointments;
    private readonly DoctorManager _doctors;
    private readonly PatientManager _patients;

    public AnalyticsManager(AppointmentManager appointments, DoctorManager doctors, PatientManager patients)
    {
        _appointments = appointments;
        _doctors = doctors;
        _patients = patients;
    }

    public IEnumerable<DoctorStats> ComputeDoctorStats()
    {
        Appointment[] appointments = _appointments.GetAll();

        foreach (Doctor doctor in _doctors.GetAll())
        {
            int count = 0;
            decimal revenue = 0m;
            DateTime last = DateTime.MinValue;

            foreach (Appointment appointment in appointments)
            {
                if (appointment.DoctorId != doctor.Id)
                {
                    continue;
                }

                count++;
                revenue += appointment.GetCost();
                if (appointment.ScheduledAt > last)
                {
                    last = appointment.ScheduledAt;
                }
            }

            yield return new DoctorStats(doctor.Id, doctor.FullName, count, revenue, last);
        }
    }

    public IEnumerable<PatientStats> ComputePatientStats()
    {
        Appointment[] appointments = _appointments.GetAll();

        foreach (Patient patient in _patients.GetAll())
        {
            int count = 0;
            decimal spent = 0m;
            DateTime last = DateTime.MinValue;

            foreach (Appointment appointment in appointments)
            {
                if (appointment.PatientId != patient.Id)
                {
                    continue;
                }

                count++;
                spent += appointment.GetCost();
                if (appointment.ScheduledAt > last)
                {
                    last = appointment.ScheduledAt;
                }
            }

            yield return new PatientStats(patient.Id, patient.FullName, count, spent, last);
        }
    }
}
