namespace FourLines.Domain.Results.Facilities;

public class FacilityErrorResults
{
    public static readonly Error OwnerDoesNotExists = new("FacilityCreation.OwnerDoesNotExists", "The specified owner does not exist.");

    public static readonly Error FacilityDoesNotExist = new("FacilityUpdate.FacilityDoesNotExist", "The specified facility does not exist.");
}
