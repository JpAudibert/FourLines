using FourLines.Domain.Models;
using FourLines.Infrastructure.Contexts;
using FourLines.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;

namespace FourLines.Tests;

public class StandardRepositoryE2ETests
{
    private static FourLinesContext CreateInMemoryContext(string dbName)
    {
        DbContextOptions<FourLinesContext> options = new DbContextOptionsBuilder<FourLinesContext>()
            .UseInMemoryDatabase(dbName)
            .Options;

        return new FourLinesContext(options) { Configuration = new ConfigurationBuilder().AddInMemoryCollection().Build() };
    }

    [Fact]
    public async Task StandardRepository_CRUD_Workflow()
    {
        // Arrange
        FourLinesContext context = CreateInMemoryContext("StandardRepoDb");
        NullLogger<StandardRepository<User>> logger = new();
        StandardRepository<User> repo = new(context, logger);
        Guid id = Guid.NewGuid();

        // Act & Assert
        // Add
        User user = new()
        {
            Name = "Alice",
            Email = "alice@example.com",
            PasswordHash = "hash",
            Birthday = DateOnly.FromDateTime(DateTime.UtcNow),
            Phone = "123",
            RegistrationNumber = "R1",
            IsActive = true,
            RoleId = id
        };
        await repo.AddAsync(user);

        // GetAll
        IEnumerable<User> all = [.. (await repo.GetAllAsync())];
        Assert.Single(all);

        User? added = all.First();
        Assert.Equal("Alice", added.Name);

        // GetEntity
        var fetched = await repo.GetEntityAsync(added.Id);
        Assert.NotNull(fetched);
        Assert.Equal(added.Email, fetched!.Email);

        // Update
        var updatedUser = new User
        {
            Id = fetched.Id,
            Name = "Alice Updated",
            Email = fetched.Email,
            PasswordHash = fetched.PasswordHash,
            Birthday = fetched.Birthday,
            Phone = fetched.Phone,
            RegistrationNumber = fetched.RegistrationNumber,
            IsActive = fetched.IsActive,
            RoleId = fetched.RoleId
        };

        await repo.UpdateAsync(updatedUser);

        User? updated = await repo.GetEntityAsync(fetched.Id);
        Assert.Equal("Alice Updated", updated!.Name);

        // Delete
        await repo.DeleteAsync(fetched.Id);
        User? afterDelete = await repo.GetEntityAsync(fetched.Id);
        Assert.Null(afterDelete);
    }
}
