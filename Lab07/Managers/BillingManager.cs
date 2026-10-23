using ClinicApp.Interfaces;
using ClinicApp.Models;

namespace ClinicApp.Managers;

public class BillingManager
{
    private readonly AppointmentManager _appointments;

    public BillingManager(AppointmentManager appointments)
    {
        _appointments = appointments;
    }

    private IPayable[] CollectUnpaid(int patientId, bool onlyThisPatient)
    {
        Appointment[] all = _appointments.GetAll();

        int matches = 0;
        for (int i = 0; i < all.Length; i++)
        {
            if (!all[i].IsPaid && !all[i].IsCancelled && (!onlyThisPatient || all[i].PatientId == patientId))
            {
                matches++;
            }
        }

        IPayable[] result = new IPayable[matches];
        int index = 0;
        for (int i = 0; i < all.Length; i++)
        {
            if (!all[i].IsPaid && !all[i].IsCancelled && (!onlyThisPatient || all[i].PatientId == patientId))
            {
                result[index++] = all[i];
            }
        }

        return result;
    }

    private static decimal SumCost(IPayable[] items)
    {
        decimal total = 0m;
        for (int i = 0; i < items.Length; i++)
        {
            total += items[i].GetCost();
        }

        return total;
    }

    public IPayable[] GetAllUnpaid()
    {
        return CollectUnpaid(0, false);
    }

    public IPayable[] GetUnpaidByPatient(int patientId)
    {
        return CollectUnpaid(patientId, true);
    }

    public decimal GetTotalDebt()
    {
        return SumCost(GetAllUnpaid());
    }

    public decimal GetPatientDebt(int patientId)
    {
        return SumCost(GetUnpaidByPatient(patientId));
    }

    public bool PayAppointment(int appointmentId)
    {
        Appointment[] all = _appointments.GetAll();

        for (int i = 0; i < all.Length; i++)
        {
            if (all[i].Id != appointmentId)
            {
                continue;
            }

            if (all[i].IsPaid || all[i].IsCancelled)
            {
                return false;
            }

            all[i].MarkPaid();
            return true;
        }

        return false;
    }

    public void DisplayUnpaid(IPayable[] items)
    {
        if (items.Length == 0)
        {
            Console.WriteLine("Немає неоплачених записів.");
            return;
        }

        foreach (IPayable item in items)
        {
            Console.WriteLine($"{item} | Сума: {item.GetCost().ToString("F2")} грн");
        }
    }
}
