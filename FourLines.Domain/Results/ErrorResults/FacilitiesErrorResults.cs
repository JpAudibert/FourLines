namespace FourLines.Domain.Results.ErrorResults;

public class FacilitiesErrorResults
{
    public static readonly Error CreateOwnerDoesNotExists = new("FacilityCreation.OwnerDoesNotExists", "The specified owner does not exist.");

    public static readonly Error UpdateFacilityDoesNotExist = new("FacilityUpdate.FacilityDoesNotExist", "The specified facility does not exist.");
    public static readonly Error UpdateEmptyOwnerId = new("FacilityUpdate.EmptyOwnerId", "Owner Id is empty");

    public static readonly Error DeleteFacilityDoesNotExist = new("FacilityDelete.FacilityDoesNotExist", "The specified facility does not exist.");

    public static readonly Error RetrieveOwnerDoesNotExists = new("FacilityRetrieval.OwnerDoesNotExists", "The specified owner does not exist.");
    public static readonly Error RetrieveFacilityDoesNotExist = new("FacilityRetrieval.FacilityDoesNotExist", "The specified facility does not exist.");

    public static readonly Error RetrieveNoFacilities = new("FacilityRetrieval.NoFacilities", "There are no facilities.");

}
