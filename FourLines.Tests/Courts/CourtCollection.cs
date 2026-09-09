namespace FourLines.Tests.Courts;

[CollectionDefinition(Name)]
public class CourtCollection : ICollectionFixture<CourtFixture>
{
    public const string Name = "Court";
}
