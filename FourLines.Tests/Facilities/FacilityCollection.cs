namespace FourLines.Tests.Facilities;

[CollectionDefinition(Name)]
public class FacilitySchedulesCollection : ICollectionFixture<FacilitySchedulesFixture>
{
    public const string Name = "Facility";
}
