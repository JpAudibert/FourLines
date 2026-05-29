using FourLines.Api.Contexts;
using FourLines.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace FourLines.Api.Seeders;

public sealed class UserSeeder : ISeeder
{
    public async Task SeedAsync(FourLinesContext context)
    {
        DateTimeOffset now = DateTimeOffset.UtcNow;

        var users = new[]
        {
            new
            {
                RoleName = "Admin",
                Name = "Admin User",
                Email = "admin@fourlines.local",
                PasswordHash = "seeded-password-hash",
                Birthday = new DateOnly(1990, 1, 1),
                Phone = "11999999999",
                RegistrationNumber = "ADM001",
                IsActive = true,
                CreatedAt = now,
                UpdatedAt = now
            },
            new
            {
                RoleName = "Manager",
                Name = "Manager User",
                Email = "manager@fourlines.local",
                PasswordHash = "seeded-password-hash",
                Birthday = new DateOnly(1988, 5, 12),
                Phone = "11988887777",
                RegistrationNumber = "MGR001",
                IsActive = true,
                CreatedAt = now,
                UpdatedAt = now
            },
            new
            {
                RoleName = "User",
                Name = "Player User",
                Email = "player@fourlines.local",
                PasswordHash = "seeded-password-hash",
                Birthday = new DateOnly(1995, 8, 23),
                Phone = "11977776666",
                RegistrationNumber = "USR001",
                IsActive = true,
                CreatedAt = now,
                UpdatedAt = now
            },
            new
            {
                RoleName = "Guest",
                Name = "Guest User",
                Email = "guest@fourlines.local",
                PasswordHash = "seeded-password-hash",
                Birthday = new DateOnly(2000, 1, 15),
                Phone = "11966665555",
                RegistrationNumber = "GST001",
                IsActive = false,
                CreatedAt = now,
                UpdatedAt = now
            }
        };

        foreach (var user in users)
        {
            bool exists = await context.Users.AnyAsync(existingUser => existingUser.Email == user.Email);

            if (exists)
            {
                continue;
            }

            Role? role = await context.Roles.FirstOrDefaultAsync(existingRole => existingRole.Name == user.RoleName);

            if (role is null)
            {
                continue;
            }

            await context.Users.AddAsync(new User
            {
                RoleId = role.Id,
                Name = user.Name,
                Email = user.Email,
                PasswordHash = user.PasswordHash,
                Birthday = user.Birthday,
                Phone = user.Phone,
                RegistrationNumber = user.RegistrationNumber,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            });
        }

        await context.SaveChangesAsync();
    }
}