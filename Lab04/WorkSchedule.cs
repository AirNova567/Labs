namespace Lab04;

public struct WorkSchedule
{
    public int Start { get; }
    public int End { get; }

    public int HoursPerDay => End - Start;

    public string Display => $"{Start:D2}:00–{End:D2}:00";

    public bool IsNow => Contains(DateTime.Now.Hour);

    public WorkSchedule(int start, int end)
    {
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
