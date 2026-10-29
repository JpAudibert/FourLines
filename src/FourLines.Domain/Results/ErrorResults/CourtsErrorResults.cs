namespace FourLines.Domain.Results.ErrorResults;

public static class CourtsErrorResults
{
    public static readonly Error CreateUnknownFacility = new("CourtCreation.UnknownFacility", "The specified facility does not exist.");
    public static readonly Error CreateUnknownSport = new("CourtCreation.UnknownSport", "The specified sport does not exist.");

    public static readonly Error UpdateUnknownFacility = new("CourtUpdate.UnknownFacility", "The specified facility does not exist.");
    public static readonly Error UpdateCourtDoesNotExist = new("CourtUpdate.CourtDoesNotExist", "The specified court does not exist.");

    public static readonly Error DeleteCourtDoesNotExist = new("CourtDelete.CourtDoesNotExist", "The specified court does not exist.");

    public static readonly Error RetrieveGetCourtDoesNotExist = new("CourtRetrieve.CourtDoesNotExist", "The specified court does not exist.");
}
