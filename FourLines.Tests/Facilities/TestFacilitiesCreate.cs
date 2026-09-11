using FourLines.Application.DTOs.Facilities.Interfaces;
using FourLines.Application.Interfaces;
using FourLines.Domain.Models;
using FourLines.Domain.Results;
using FourLines.Domain.Results.ErrorResults;
using FourLines.Tests.Shared;

namespace FourLines.Tests.Facilities;

public record TestCreateFacilityDTO : ICreateFacilityDTO
{
    public Guid OwnerId { get; init; } = default!;
    public string Name { get; init; } = default!;
    public string Address { get; init; } = default!;
    public string City { get; init; } = default!;
    public string State { get; init; } = default!;
    public string ZipCode { get; init; } = default!;
    public string RegistrationNumber { get; init; } = default!;
}

[Collection(FourLinesCollection.Name)]
public class TestFacilitiesCreate(FourLinesFixture fixtures)
{
    private static readonly TestCreateFacilityDTO _createFacilityTest = new()
    {
        Name = "Test Facility",
        Address = "123 Test St",
        City = "Test City",
        State = "TS",
        ZipCode = "12345",
        RegistrationNumber = "1234555555",
        OwnerId = TestDataSource.UserOwner.Id,
    };

    [Fact]
    public async Task Should_CreateFacility()
    {
        // Arrange
        await using var scope = fixtures.CreateAsyncServiceScope();

        IFacilityHandler facilityHandler = fixtures.ServiceProvider.GetRequiredService<IFacilityHandler>();

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
        await using var scope = fixtures.CreateAsyncServiceScope();

        IFacilityHandler facilityHandler = fixtures.ServiceProvider.GetRequiredService<IFacilityHandler>();
        TestCreateFacilityDTO facilityWithNoOwner = _createFacilityTest with { OwnerId = Guid.NewGuid() };

        // Act
        Result<Facility> result = await facilityHandler.Create(facilityWithNoOwner);

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(FacilitiesErrorResults.CreateOwnerDoesNotExists, result.Error);
    }
}
