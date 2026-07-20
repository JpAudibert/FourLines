using FourLines.Application.DTOs.Facilities;
using FourLines.Application.Handlers;
using FourLines.Domain.Models;
using FourLines.Domain.Results;
using FourLines.Domain.Results.ErrorResults;
using FourLines.Tests.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace FourLines.Tests.Facilities;

public class TestFacilitiesCreate(InMemoryFixtures fixtures) : IClassFixture<InMemoryFixtures>
{
    private readonly InMemoryFixtures _fixtures = fixtures;
    
    private static CreateFacilityDTO _createFacilityTest = new()
    {
        Name = "Test Facility",
        Address = "123 Test St",
        City = "Test City",
        State = "TS",
        ZipCode = "12345",
        RegistrationNumber = "1234555555",
        OwnerId = InMemoryDataSource.userOwner.Id,
    };

    [Fact]
    public async Task Should_CreateFacility()
    {
        // Arrange
        await _fixtures.CreateEntityInMemory<Role>(InMemoryDataSource.roleOwner);
        await _fixtures.CreateEntityInMemory<User>(InMemoryDataSource.userOwner);

        FacilityHandler facilityHandler =
            _fixtures.ServiceProvider.GetRequiredService<FacilityHandler>();

        // Act
        Result<Facility> result = await facilityHandler.Create(_createFacilityTest);

        // Assert
        Assert.NotNull(result.Value);
        Assert.IsType<Facility>(result.Value);
        Assert.Equal(_createFacilityTest.Name, result.Value.Name);
        Assert.Equal(_createFacilityTest.Address, result.Value.Address);
        Assert.Equal(_createFacilityTest.City, result.Value.City);
        Assert.Equal(_createFacilityTest.State, result.Value.State);
        Assert.Equal(_createFacilityTest.ZipCode, result.Value.ZipCode);
        Assert.Equal(_createFacilityTest.RegistrationNumber, result.Value.RegistrationNumber);
        Assert.Equal(_createFacilityTest.OwnerId, result.Value.OwnerId);
    }

    [Fact]
    public async Task Should_Not_CreateFacility()
    {
        // Arrange

        await _fixtures.CreateEntityInMemory<Role>(InMemoryDataSource.rolePlayer);
        await _fixtures.CreateEntityInMemory<User>(InMemoryDataSource.userPlayer);

        FacilityHandler facilityHandler =
            _fixtures.ServiceProvider.GetRequiredService<FacilityHandler>();

        // Act
        Result<Facility> result = await facilityHandler.Create(_createFacilityTest);

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(FacilitiesErrorResults.CreateOwnerDoesNotExists, result.Error);
    }
}
