namespace FourLines.Tests.Facilities;

[CollectionDefinition(Name)]
public class FacilityCollection : ICollectionFixture<FacilityFixture>
{
    public const string Name = "Facility";
}
