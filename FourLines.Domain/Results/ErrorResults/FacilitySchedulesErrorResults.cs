namespace FourLines.Domain.Results.ErrorResults;

public class FacilitySchedulesErrorResults
{
    public static readonly Error CreateFacilitySchedules = new("FacilitySchedulesCreation.UnknownFacility", "No facility found for the specified facility and owner.");

    public static readonly Error UpdateFacilitySchedules = new("FacilitySchedulesUpdate.UnknownFacility", "No facility found for the specified facility and owner.");
    public static readonly Error UpdateUnknownSchedules = new("FacilitySchedulesUpdate.UnknownSchedules", "No schedules found for the specified facility and owner.");

    public static readonly Error DeleteUnknownSchedules = new("FacilitySchedulesdelete.UnknownSchedules", "No schedules found for the specified facility and owner.");

    public static readonly Error RetrieveFacilitySchedules = new("FacilitySchedulesRetrieve.UnknownSchedules", "No schedules found for the specified facility and owner.");
}
