using TimeZoneConverter;

namespace FourLines.Application.Providers;

public sealed class TimeZoneProvider : ITimeZoneProvider
{
    public static DateTimeOffset ToUtc(string timeZoneId, DateOnly date, TimeOnly time)
    {
        TimeZoneInfo timeZone = TZConvert.GetTimeZoneInfo(timeZoneId);
        DateTime localDateTime = date.ToDateTime(time);

        DateTime utc = TimeZoneInfo.ConvertTimeToUtc(localDateTime, timeZone);

        return new DateTimeOffset(utc, TimeSpan.Zero);
    }

    public static DateTimeOffset ToFacilityTime(string timeZoneId, DateTimeOffset utcDateTime)
    {
        TimeZoneInfo timeZone = TZConvert.GetTimeZoneInfo(timeZoneId);

        return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime.UtcDateTime, timeZone);
    }

    public static TimeOnly ToFacilityTimeOnly(string timeZoneId, DateTimeOffset utcDateTime)
    {
        TimeZoneInfo timeZone = TZConvert.GetTimeZoneInfo(timeZoneId);

        DateTime facilityDateTime = TimeZoneInfo.ConvertTimeFromUtc(
            utcDateTime.UtcDateTime,
            timeZone
        );

        return TimeOnly.FromDateTime(facilityDateTime);
    }

    public static TimeRange EnsureUtcForTimeRange(string timeZoneId, TimeRange period)
    {
        DateTimeOffset utcStart = ToUtc(
            timeZoneId,
            DateOnly.FromDateTime(period.Start.Date),
            TimeOnly.FromDateTime(period.Start.Date)
        );
        DateTimeOffset utcEnd = ToUtc(
            timeZoneId,
            DateOnly.FromDateTime(period.End.Date),
            TimeOnly.FromDateTime(period.End.Date)
        );

        return new TimeRange(utcStart, utcEnd);
    }
}
