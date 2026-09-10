using FourLines.Application.DTOs.Facilities.Interfaces;
using FourLines.Application.Interfaces;
using FourLines.Domain.Models;
using FourLines.Domain.Results;
using FourLines.Domain.Results.ErrorResults;
using FourLines.Tests.Shared;

namespace FourLines.Tests.Facilities;

public record TestUpdateFacilityDTO : IUpdateFacilityDTO
{
    public Guid Id { get; init; } = default!;
    public Guid OwnerId { get; init; } = default!;
    public string Name { get; init; } = default!;
    public string Address { get; init; } = default!;
    public string City { get; init; } = default!;
    public string State { get; init; } = default!;
    public string ZipCode { get; init; } = default!;
    public string RegistrationNumber { get; init; } = default!;
}

[Collection(FourLinesCollection.Name)]
public class TestFacilitiesUpdate(FourLinesFixture fixtures)
{
    private static readonly TestUpdateFacilityDTO _updateFacilityTest = new()
    {
        Id = TestDataSource.Facility2.Id,
        Name = "Test Updated Facility",
        Address = "123 Test St",
        City = "Test City",
        State = "TS",
        ZipCode = "12345",
        RegistrationNumber = "1111111111",
        OwnerId = TestDataSource.UserOwner.Id,
    };

    [Fact]
    public async Task Should_UpdateFacility()
    {
        // Arrange
        using var context = fixtures.CreateContext();
        Facility toBeUpdatedFacility = await DbOperations.CreateRecord(TestDataSource.ToBeUpdatedFacility, context);

        IFacilityHandler facilityHandler = fixtures.ServiceProvider.GetRequiredService<IFacilityHandler>();

        TestUpdateFacilityDTO updateFacilityDTO = new()
        {
            Id = toBeUpdatedFacility.Id,
            Name = "Test Updated Facility",
            Address = "123 Test St",
            City = "Test City",
            State = "TS",
            ZipCode = "12345",
            RegistrationNumber = "1111111111",
            OwnerId = TestDataSource.UserOwner.Id,
        };

        // Act
        Result<Facility> result = await facilityHandler.Update(_updateFacilityTest);

        // Assert
        Assert.NotNull(result.Value);
        Assert.IsType<Facility>(result.Value);
        Assert.Equal(_updateFacilityTest.Name, result.Value.Name);
        Assert.Equal(_updateFacilityTest.Address, result.Value.Address);
        Assert.Equal(_updateFacilityTest.City, result.Value.City);
        Assert.Equal(_updateFacilityTest.State, result.Value.State);
        Assert.Equal(_updateFacilityTest.ZipCode, result.Value.ZipCode);
        Assert.Equal(_updateFacilityTest.RegistrationNumber, result.Value.RegistrationNumber);
        Assert.Equal(_updateFacilityTest.OwnerId, result.Value.OwnerId);

        await DbOperations.RemoveRecord<Facility>(toBeUpdatedFacility.Id, context);
    }

    [Fact]
    public async Task Should_Not_FindOwnerFacility()
    {
        // Arrange
        IFacilityHandler facilityHandler = fixtures.ServiceProvider.GetRequiredService<IFacilityHandler>();
        TestUpdateFacilityDTO facilityWithNoOwnerId = _updateFacilityTest with { OwnerId = Guid.Empty };

        // Act
        Result<Facility> result = await facilityHandler.Update(facilityWithNoOwnerId);

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(FacilitiesErrorResults.UpdateEmptyOwnerId, result.Error);
    }

    [Fact]
    public async Task Should_Not_AffectAnyRowFacility()
    {
        // Arrange
        IFacilityHandler facilityHandler = fixtures.ServiceProvider.GetRequiredService<IFacilityHandler>();
        TestUpdateFacilityDTO unknownFacility = _updateFacilityTest with { Id = Guid.NewGuid() };

        // Act
        Result<Facility> result = await facilityHandler.Update(unknownFacility);

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(FacilitiesErrorResults.UpdateFacilityDoesNotExist, result.Error);
    }
}
