using FourLines.Application.Interfaces;
using FourLines.Domain.Models;
using FourLines.Domain.Results;
using FourLines.Domain.Results.ErrorResults;
using FourLines.Tests.Shared;

namespace FourLines.Tests.Facilities;

[Collection(FacilityCollection.Name)]
public class TestFacilitiesRead(FacilityFixture fixtures)
{
    [Fact]
    public async Task Should_GetAllFacilities()
    {
        // Arrange
        await using (var context = fixtures.CreateContext())
        {
            await DbOperations.CreateRecord<Role>(TestDataSource.RoleOwner, context);
            await DbOperations.CreateRecord<User>(TestDataSource.UserOwner, context);
            await DbOperations.CreateRecord<Facility>(TestDataSource.DefaultFacility, context);
            await DbOperations.CreateRecord<Facility>(TestDataSource.Facility2, context);
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
            await DbOperations.RemoveAllRecords<Facility>(context);
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
            await DbOperations.CreateRecord<Role>(TestDataSource.RoleOwner, context);
            await DbOperations.CreateRecord<User>(TestDataSource.UserOwner, context);
            await DbOperations.CreateRecord<Facility>(TestDataSource.DefaultFacility, context);
            await DbOperations.CreateRecord<Facility>(TestDataSource.Facility2, context);
        }

        IFacilityHandler facilityHandler =
            fixtures.ServiceProvider.GetRequiredService<IFacilityHandler>();

        // Act
        Result<IEnumerable<Facility>> result = await facilityHandler.GetFacilitiesFromOwner(
            TestDataSource.DefaultFacility.OwnerId
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
            await DbOperations.CreateRecord<Role>(TestDataSource.RoleOwner, context);
            await DbOperations.CreateRecord<User>(TestDataSource.UserOwner, context);
            await DbOperations.CreateRecord<Facility>(TestDataSource.DefaultFacility, context);
        }

        IFacilityHandler facilityHandler =
            fixtures.ServiceProvider.GetRequiredService<IFacilityHandler>();

        // Act
        Result<Facility> result = await facilityHandler.GetFacilityFromOwner(
            TestDataSource.UserOwner.Id,
            TestDataSource.DefaultFacility.Id
        );

        // Assert
        Assert.NotNull(result.Value);
        Assert.Equal(TestDataSource.DefaultFacility.Name, result.Value.Name);
        Assert.Equal(TestDataSource.DefaultFacility.Address, result.Value.Address);
        Assert.Equal(TestDataSource.DefaultFacility.City, result.Value.City);
        Assert.Equal(TestDataSource.DefaultFacility.State, result.Value.State);
        Assert.Equal(TestDataSource.DefaultFacility.ZipCode, result.Value.ZipCode);
        Assert.Equal(
            TestDataSource.DefaultFacility.RegistrationNumber,
            result.Value.RegistrationNumber
        );
        Assert.Equal(TestDataSource.DefaultFacility.OwnerId, result.Value.OwnerId);
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
            await DbOperations.CreateRecord<Role>(TestDataSource.RoleOwner, context);
            await DbOperations.CreateRecord<User>(TestDataSource.UserOwner, context);
        }

        IFacilityHandler facilityHandler =
            fixtures.ServiceProvider.GetRequiredService<IFacilityHandler>();

        // Act
        Result<Facility> result = await facilityHandler.GetFacilityFromOwner(
            TestDataSource.DefaultFacility.OwnerId,
            Guid.NewGuid()
        );

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(FacilitiesErrorResults.RetrieveFacilityDoesNotExist, result.Error);
    }
}
