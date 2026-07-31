using FourLines.Application.Handlers;
using FourLines.Domain.Models;
using FourLines.Domain.Results;
using FourLines.Domain.Results.ErrorResults;
using FourLines.Tests.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace FourLines.Tests.Reservations;

public class TestReservationsRead(InMemoryFixtures fixtures) : IClassFixture<InMemoryFixtures>
{
    private readonly InMemoryFixtures _fixtures = fixtures;

    [Fact]
    public async Task Should_GetAllReservations()
    {
        // Arrange
        await _fixtures.CreateEntityInMemory<Role>(InMemoryDataSource.roleOwner);
        await _fixtures.CreateEntityInMemory<User>(InMemoryDataSource.userOwner);
        await _fixtures.CreateEntityInMemory<Facility>(InMemoryDataSource.facility1);
        await _fixtures.CreateEntityInMemory<Facility>(InMemoryDataSource.facility2);

        FacilityHandler facilityHandler =
            _fixtures.ServiceProvider.GetRequiredService<FacilityHandler>();

        // Act
        Result<IEnumerable<Facility>> result = await facilityHandler.GetAllFacilities();

        // Assert
        Assert.NotEmpty(result.Value);
        Assert.Equal(2, result.Value.Count());
    }

    [Fact]
    public async Task Should_Not_GetAllReservations()
    {
        // Arrange
        await _fixtures.RemoveAllDataFromMemory<Facility>();

        FacilityHandler facilityHandler =
            _fixtures.ServiceProvider.GetRequiredService<FacilityHandler>();

        // Act
        Result<IEnumerable<Facility>> result = await facilityHandler.GetAllFacilities();

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(FacilitiesErrorResults.RetrieveNoFacilities, result.Error);
    }

    [Fact]
    public async Task Should_GetReservations()
    {
        // Arrange
        await _fixtures.CreateEntityInMemory<Role>(InMemoryDataSource.roleOwner);
        await _fixtures.CreateEntityInMemory<User>(InMemoryDataSource.userOwner);
        await _fixtures.CreateEntityInMemory<Facility>(InMemoryDataSource.facility1);
        await _fixtures.CreateEntityInMemory<Facility>(InMemoryDataSource.facility2);

        FacilityHandler facilityHandler =
            _fixtures.ServiceProvider.GetRequiredService<FacilityHandler>();

        // Act
        Result<IEnumerable<Facility>> result = await facilityHandler.GetFacilitiesFromOwner(
            InMemoryDataSource.userOwner.Id
        );

        // Assert
        Assert.NotEmpty(result.Value);
        Assert.Equal(2, result.Value.Count());
    }

    [Fact]
    public async Task Should_Not_GetReservations()
    {
        // Arrange
        FacilityHandler facilityHandler =
            _fixtures.ServiceProvider.GetRequiredService<FacilityHandler>();

        // Act
        Result<IEnumerable<Facility>> result = await facilityHandler.GetFacilitiesFromOwner(
            Guid.NewGuid()
        );

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(FacilitiesErrorResults.RetrieveOwnerDoesNotExists, result.Error);
    }

    [Fact]
    public async Task Should_GetFacilitySchedules()
    {
        // Arrange
       
        await _fixtures.CreateEntityInMemory<Role>(InMemoryDataSource.roleOwner);
        await _fixtures.CreateEntityInMemory<User>(InMemoryDataSource.userOwner);
        await _fixtures.CreateEntityInMemory<Facility>(InMemoryDataSource.facility1);

        FacilityHandler facilityHandler =
            _fixtures.ServiceProvider.GetRequiredService<FacilityHandler>();

        // Act
        Result<Facility> result = await facilityHandler.GetFacilityFromOwner(
            InMemoryDataSource.userOwner.Id,
            InMemoryDataSource.facility1.Id
        );

        // Assert
        Assert.NotNull(result.Value);
        Assert.Equal(InMemoryDataSource.facility1.Name, result.Value.Name);
        Assert.Equal(InMemoryDataSource.facility1.Address, result.Value.Address);
        Assert.Equal(InMemoryDataSource.facility1.City, result.Value.City);
        Assert.Equal(InMemoryDataSource.facility1.State, result.Value.State);
        Assert.Equal(InMemoryDataSource.facility1.ZipCode, result.Value.ZipCode);
        Assert.Equal(
            InMemoryDataSource.facility1.RegistrationNumber,
            result.Value.RegistrationNumber
        );
        Assert.Equal(InMemoryDataSource.facility1.OwnerId, result.Value.OwnerId);
    }

    [Fact]
    public async Task Should_Not_GetOwnerFacilitySchedules()
    {
        // Arrange
        FacilityHandler facilityHandler =
            _fixtures.ServiceProvider.GetRequiredService<FacilityHandler>();

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
    public async Task Should_Not_GetFacilitySchedules()
    {
        // Arrange
        await _fixtures.CreateEntityInMemory<Role>(InMemoryDataSource.roleOwner);
        await _fixtures.CreateEntityInMemory<User>(InMemoryDataSource.userOwner);

        FacilityHandler facilityHandler =
            _fixtures.ServiceProvider.GetRequiredService<FacilityHandler>();

        // Act
        Result<Facility> result = await facilityHandler.GetFacilityFromOwner(
            InMemoryDataSource.userOwner.Id,
            Guid.NewGuid()
        );

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(FacilitiesErrorResults.RetrieveFacilityDoesNotExist, result.Error);
    }
}
