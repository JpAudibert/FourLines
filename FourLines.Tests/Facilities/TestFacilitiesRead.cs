using FourLines.Application.Interfaces;
using FourLines.Domain.Models;
using FourLines.Domain.Results;
using FourLines.Domain.Results.ErrorResults;
using FourLines.Tests.Shared;

namespace FourLines.Tests.Facilities;

public class TestFacilitiesRead(InMemoryFixtures fixtures) : IClassFixture<InMemoryFixtures>
{
    [Fact]
    public async Task Should_GetAllFacilities()
    {
        // Arrange
        await using (var context = fixtures.CreateContext())
        {
            await DbOperations.CreateEntityInMemory<Role>(InMemoryDataSource.RoleOwner, context);
            await DbOperations.CreateEntityInMemory<User>(InMemoryDataSource.UserOwner, context);
            await DbOperations.CreateEntityInMemory<Facility>(InMemoryDataSource.Facility1, context);
            await DbOperations.CreateEntityInMemory<Facility>(InMemoryDataSource.Facility2, context);
        }

        IFacilityHandler facilityHandler =
            fixtures.ServiceProvider.GetRequiredService<IFacilityHandler>();

        // Act
        Result<IEnumerable<Facility>> result = await facilityHandler.GetAllFacilities();

        // Assert
        Assert.NotEmpty(result.Value);
        Assert.Equal(2, result.Value.Count());
    }

    [Fact]
    public async Task Should_Not_GetAllFacilities()
    {
        // Arrange
        await using (var context = fixtures.CreateContext())
        {
            await DbOperations.RemoveAllDataFromMemory<Facility>(context);
        }

        IFacilityHandler facilityHandler =
            fixtures.ServiceProvider.GetRequiredService<IFacilityHandler>();

        // Act
        Result<IEnumerable<Facility>> result = await facilityHandler.GetAllFacilities();

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(FacilitiesErrorResults.RetrieveNoFacilities, result.Error);
    }

    [Fact]
    public async Task Should_GetFacilities()
    {
        // Arrange
        await using (var context = fixtures.CreateContext())
        {
            await DbOperations.CreateEntityInMemory<Role>(InMemoryDataSource.RoleOwner, context);
            await DbOperations.CreateEntityInMemory<User>(InMemoryDataSource.UserOwner, context);
            await DbOperations.CreateEntityInMemory<Facility>(InMemoryDataSource.Facility1, context);
            await DbOperations.CreateEntityInMemory<Facility>(InMemoryDataSource.Facility2, context);
        }

        IFacilityHandler facilityHandler =
            fixtures.ServiceProvider.GetRequiredService<IFacilityHandler>();

        // Act
        Result<IEnumerable<Facility>> result = await facilityHandler.GetFacilitiesFromOwner(
            InMemoryDataSource.UserOwner.Id
        );

        // Assert
        Assert.NotEmpty(result.Value);
        Assert.Equal(2, result.Value.Count());
    }

    [Fact]
    public async Task Should_Not_GetFacilities()
    {
        // Arrange
        IFacilityHandler facilityHandler =
            fixtures.ServiceProvider.GetRequiredService<IFacilityHandler>();

        // Act
        Result<IEnumerable<Facility>> result = await facilityHandler.GetFacilitiesFromOwner(
            Guid.NewGuid()
        );

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(FacilitiesErrorResults.RetrieveOwnerDoesNotExists, result.Error);
    }

    [Fact]
    public async Task Should_GetFacility()
    {
        // Arrange
        await using (var context = fixtures.CreateContext())
        {
            await DbOperations.CreateEntityInMemory<Role>(InMemoryDataSource.RoleOwner, context);
            await DbOperations.CreateEntityInMemory<User>(InMemoryDataSource.UserOwner, context);
            await DbOperations.CreateEntityInMemory<Facility>(InMemoryDataSource.Facility1, context);
        }

        IFacilityHandler facilityHandler =
            fixtures.ServiceProvider.GetRequiredService<IFacilityHandler>();

        // Act
        Result<Facility> result = await facilityHandler.GetFacilityFromOwner(
            InMemoryDataSource.UserOwner.Id,
            InMemoryDataSource.Facility1.Id
        );

        // Assert
        Assert.NotNull(result.Value);
        Assert.Equal(InMemoryDataSource.Facility1.Name, result.Value.Name);
        Assert.Equal(InMemoryDataSource.Facility1.Address, result.Value.Address);
        Assert.Equal(InMemoryDataSource.Facility1.City, result.Value.City);
        Assert.Equal(InMemoryDataSource.Facility1.State, result.Value.State);
        Assert.Equal(InMemoryDataSource.Facility1.ZipCode, result.Value.ZipCode);
        Assert.Equal(
            InMemoryDataSource.Facility1.RegistrationNumber,
            result.Value.RegistrationNumber
        );
        Assert.Equal(InMemoryDataSource.Facility1.OwnerId, result.Value.OwnerId);
    }

    [Fact]
    public async Task Should_Not_GetOwnerFacility()
    {
        // Arrange
        IFacilityHandler facilityHandler =
            fixtures.ServiceProvider.GetRequiredService<IFacilityHandler>();

        // Act
        Result<Facility> result = await facilityHandler.GetFacilityFromOwner(
            Guid.NewGuid(),
            Guid.NewGuid()
        );

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(FacilitiesErrorResults.RetrieveOwnerDoesNotExists, result.Error);
    }

    [Fact]
    public async Task Should_Not_GetFacility()
    {
        // Arrange
        await using (var context = fixtures.CreateContext())
        {
            await DbOperations.CreateEntityInMemory<Role>(InMemoryDataSource.RoleOwner, context);
            await DbOperations.CreateEntityInMemory<User>(InMemoryDataSource.UserOwner, context);
        }

        IFacilityHandler facilityHandler =
            fixtures.ServiceProvider.GetRequiredService<IFacilityHandler>();

        // Act
        Result<Facility> result = await facilityHandler.GetFacilityFromOwner(
            InMemoryDataSource.UserOwner.Id,
            Guid.NewGuid()
        );

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(FacilitiesErrorResults.RetrieveFacilityDoesNotExist, result.Error);
    }
}
