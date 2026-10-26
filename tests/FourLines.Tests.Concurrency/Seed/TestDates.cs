namespace FourLines.Tests.Concurrency.Seed;

public static class TestDates
{
    public static readonly DateTimeOffset Now = DateTimeOffset.UtcNow;

    public static readonly DateTimeOffset Future = new(
        DateTimeOffset.UtcNow.Year + 1,
        2,
        1,
        22,
        0,
        0,
        TimeSpan.Zero
    );
}
