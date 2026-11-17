namespace ClinicApp.Models;

public struct WorkSchedule
{
    public int Start { get; }
    public int End { get; }

    public int HoursPerDay => End - Start;

    public string Display => $"{Start:D2}:00–{End:D2}:00";

    public bool IsNow => Contains(DateTime.Now.Hour);

    public WorkSchedule(int start, int end)
    {
        if (start < 0 || start > 23)
        {
            throw new ArgumentException("Початок робочого дня має бути в межах 0–23.", nameof(start));
        }

        if (end < 1 || end > 24)
        {
            throw new ArgumentException("Кінець робочого дня має бути в межах 1–24.", nameof(end));
        }

        if (start >= end)
        {
            throw new ArgumentException("Початок робочого дня має бути раніше за кінець.", nameof(start));
        }

        Start = start;
        End = end;
    }

    public bool Contains(int hour)
    {
        return hour >= Start && hour < End;
    }

    public override string ToString()
    {
        return Display + " (" + HoursPerDay + " год)";
    }
}
