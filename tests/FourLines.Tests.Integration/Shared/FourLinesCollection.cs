namespace FourLines.Tests.Integration.Shared;

[CollectionDefinition(Name, DisableParallelization = true)]
public class FourLinesCollection : ICollectionFixture<FourLinesFixture>
{
    public const string Name = "FourLinesCollection";
}
