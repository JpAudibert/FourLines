namespace FourLines.Domain.Results.ErrorResults;

public class AvailabilityErrorResults
{
    public static readonly Error NoScheduleForFacility = new("Availability.NoScheduleForFacility", "The facility has no schedules");
}
