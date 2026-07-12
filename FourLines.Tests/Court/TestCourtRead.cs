// using FourLines.Application.DTOs.Court;
// using FourLines.Application.Handlers;
// using FourLines.Domain.Constants;
// using FourLines.Domain.Models;
// using FourLines.Domain.Results;
// using FourLines.Domain.Results.ErrorResults;
// using Microsoft.Extensions.DependencyInjection;

// namespace FourLines.Tests.Court;

// public class TestCourtRead(InMemoryFixtures fixtures) : IClassFixture<InMemoryFixtures>
// {
//     private readonly InMemoryFixtures _fixtures = fixtures;

//     private static Role _testRoleOwner = new() { Name = RoleConstants.FacilityOwner };
//     private static Role _testRolePlayer = new() { Name = RoleConstants.Player };

//     private static User _testUser = new()
//     {
//         RoleId = Guid.Empty,
//         Name = "John Doe",
//         Email = "john.doe@example.com",
//         PasswordHash = "Password123!",
//         Birthday = new DateOnly(1970, 1, 1),
//         Phone = "55 54 9 9999-9999",
//         RegistrationNumber = "383.975.210-89",
//     };

//     [Fact]
//     public async Task Should_GetAllCourt()
//     {
//         // Arrange
//         Role roleOwner = await _fixtures.CreateEntityInMemory<Role>(_testRoleOwner);

//         _testUser.RoleId = roleOwner.Id;
//         await _fixtures.CreateEntityInMemory<User>(_testUser);

//         CreateFacilityDTO createFacilityTest1 = new()
//         {
//             Name = "Test Facility 1",
//             Address = "123 Test St",
//             City = "Test City",
//             State = "TS",
//             ZipCode = "12345",
//             RegistrationNumber = "1234567890",
//             OwnerId = _testUser.Id,
//         };

//         CreateFacilityDTO createFacilityTest2 = new()
//         {
//             Name = "Test Facility 2",
//             Address = "456 Test Ave",
//             City = "Test City 2",
//             State = "TS",
//             ZipCode = "12345",
//             RegistrationNumber = "0987654321",
//             OwnerId = _testUser.Id,
//         };

//         FacilityHandler facilityHandler =
//             _fixtures.ServiceProvider.GetRequiredService<FacilityHandler>();

//         await facilityHandler.Create(createFacilityTest1);
//         await facilityHandler.Create(createFacilityTest2);

//         // Act
//         Result<IEnumerable<Facility>> result = await facilityHandler.GetAllCourt();

//         // Assert
//         Assert.NotEmpty(result.Value);
//         Assert.Equal(2, result.Value.Count());
//     }

//     [Fact]
//     public async Task Should_Not_GetAllCourt()
//     {
//         // Arrange
//         FacilityHandler facilityHandler =
//             _fixtures.ServiceProvider.GetRequiredService<FacilityHandler>();
//         // Act
//         Result<IEnumerable<Facility>> result = await facilityHandler.GetAllCourt();

//         // Assert
//         Assert.Null(result.Value);
//         Assert.Equal(CourtErrorResults.RetrieveNoCourt, result.Error);
//     }

//     [Fact]
//     public async Task Should_GetCourt()
//     {
//         // Arrange
//         Role roleOwner = await _fixtures.CreateEntityInMemory<Role>(_testRoleOwner);

//         _testUser.RoleId = roleOwner.Id;
//         await _fixtures.CreateEntityInMemory<User>(_testUser);

//         CreateFacilityDTO createFacilityTest1 = new()
//         {
//             Name = "Test Facility 1",
//             Address = "123 Test St",
//             City = "Test City",
//             State = "TS",
//             ZipCode = "12345",
//             RegistrationNumber = "1234567890",
//             OwnerId = _testUser.Id,
//         };

//         CreateFacilityDTO createFacilityTest2 = new()
//         {
//             Name = "Test Facility 2",
//             Address = "456 Test Ave",
//             City = "Test City 2",
//             State = "TS",
//             ZipCode = "12345",
//             RegistrationNumber = "0987654321",
//             OwnerId = _testUser.Id,
//         };

//         FacilityHandler facilityHandler =
//             _fixtures.ServiceProvider.GetRequiredService<FacilityHandler>();

//         await facilityHandler.Create(createFacilityTest1);
//         await facilityHandler.Create(createFacilityTest2);

//         // Act
//         Result<IEnumerable<Facility>> result = await facilityHandler.GetCourtFromOwner(
//             _testUser.Id
//         );

//         // Assert
//         Assert.NotEmpty(result.Value);
//         Assert.Equal(2, result.Value.Count());
//     }

//     [Fact]
//     public async Task Should_Not_GetCourt()
//     {
//         // Arrange
//         FacilityHandler facilityHandler =
//             _fixtures.ServiceProvider.GetRequiredService<FacilityHandler>();

//         // Act
//         Result<IEnumerable<Facility>> result = await facilityHandler.GetCourtFromOwner(
//             Guid.NewGuid()
//         );

//         // Assert
//         Assert.Null(result.Value);
//         Assert.Equal(CourtErrorResults.RetrieveOwnerDoesNotExists, result.Error);
//     }

//     [Fact]
//     public async Task Should_GetFacility()
//     {
//         // Arrange
//         Role roleOwner = await _fixtures.CreateEntityInMemory<Role>(_testRoleOwner);

//         _testUser.RoleId = roleOwner.Id;
//         await _fixtures.CreateEntityInMemory<User>(_testUser);

//         CreateFacilityDTO createFacilityTest = new()
//         {
//             Name = "Test Facility 1",
//             Address = "123 Test St",
//             City = "Test City",
//             State = "TS",
//             ZipCode = "12345",
//             RegistrationNumber = "1234567890",
//             OwnerId = _testUser.Id,
//         };

//         FacilityHandler facilityHandler =
//             _fixtures.ServiceProvider.GetRequiredService<FacilityHandler>();

//         Result<Facility> resultCreate = await facilityHandler.Create(createFacilityTest);

//         // Act
//         Result<Facility> result = await facilityHandler.GetFacilityFromOwner(
//             _testUser.Id,
//             resultCreate.Value.Id
//         );

//         // Assert
//         Assert.NotNull(result.Value);
//         Assert.Equal(createFacilityTest.Name, result.Value.Name);
//         Assert.Equal(createFacilityTest.Address, result.Value.Address);
//         Assert.Equal(createFacilityTest.City, result.Value.City);
//         Assert.Equal(createFacilityTest.State, result.Value.State);
//         Assert.Equal(createFacilityTest.ZipCode, result.Value.ZipCode);
//         Assert.Equal(createFacilityTest.RegistrationNumber, result.Value.RegistrationNumber);
//         Assert.Equal(createFacilityTest.OwnerId, result.Value.OwnerId);
//     }

//     [Fact]
//     public async Task Should_Not_GetOwnerFacility()
//     {
//         // Arrange
//         FacilityHandler facilityHandler =
//             _fixtures.ServiceProvider.GetRequiredService<FacilityHandler>();

//         // Act
//         Result<Facility> result = await facilityHandler.GetFacilityFromOwner(
//             Guid.NewGuid(),
//             Guid.NewGuid()
//         );

//         // Assert
//         Assert.Null(result.Value);
//         Assert.Equal(CourtErrorResults.RetrieveOwnerDoesNotExists, result.Error);
//     }

//     [Fact]
//     public async Task Should_Not_GetFacility()
//     {
//         // Arrange
//         Role roleOwner = await _fixtures.CreateEntityInMemory<Role>(_testRoleOwner);

//         _testUser.RoleId = roleOwner.Id;
//         await _fixtures.CreateEntityInMemory<User>(_testUser);

//         FacilityHandler facilityHandler =
//             _fixtures.ServiceProvider.GetRequiredService<FacilityHandler>();

//         // Act
//         Result<Facility> result = await facilityHandler.GetFacilityFromOwner(
//             _testUser.Id,
//             Guid.NewGuid()
//         );

//         // Assert
//         Assert.Null(result.Value);
//         Assert.Equal(CourtErrorResults.RetrieveFacilityDoesNotExist, result.Error);
//     }
// }
