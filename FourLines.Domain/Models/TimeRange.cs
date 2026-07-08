namespace FourLines.Domain.Models;

public record TimeRange(DateTimeOffset Start, DateTimeOffset End)
{
    public TimeSpan Duration => End - Start;

    public bool AreDatesValid() => End > Start;
    public bool StartAndEndAreInThePast() => Start < DateTimeOffset.Now && End < DateTimeOffset.Now;
    public bool StartAndEndAreInTheSameDay() => Start.Date == End.Date;

    public bool Overlaps(TimeRange other) => Start < other.End && End > other.Start;

    public bool Contains(DateTime instant) => instant >= Start && instant < End;
    public bool Contains(TimeRange other) => Start <= other.Start && End >= other.End;
}
