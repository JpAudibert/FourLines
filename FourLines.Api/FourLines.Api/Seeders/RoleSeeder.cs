using FourLines.Api.Contexts;
using FourLines.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace FourLines.Api.Seeders;

public sealed class RoleSeeder : ISeeder
{
    public async Task SeedAsync(FourLinesContext context)
    {
        DateTimeOffset now = DateTimeOffset.UtcNow;

        var roles = new[]
        {
            new Role { Name = "Admin", CreatedAt = now, UpdatedAt = now },
            new Role { Name = "Manager", CreatedAt = now, UpdatedAt = now },
            new Role { Name = "User", CreatedAt = now, UpdatedAt = now },
            new Role { Name = "Guest", CreatedAt = now, UpdatedAt = now }
        };

        foreach (var role in roles)
        {
            bool exists = await context.Roles.AnyAsync(existingRole => existingRole.Name == role.Name);

            if (!exists)
            {
                await context.Roles.AddAsync(role);
            }
        }

        await context.SaveChangesAsync();
    }
}