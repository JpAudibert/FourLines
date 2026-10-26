namespace FourLines.Domain.Interfaces;

public interface ITimeZoneProvider
{
    static abstract DateTimeOffset ToFacilityTime(string timeZoneId, DateTimeOffset utcDateTime);
    static abstract TimeOnly ToFacilityTimeOnly(string timeZoneId, DateTimeOffset utcDateTime);
    static abstract DateTimeOffset ToUtc(string timeZoneId, DateOnly date, TimeOnly time);
}
