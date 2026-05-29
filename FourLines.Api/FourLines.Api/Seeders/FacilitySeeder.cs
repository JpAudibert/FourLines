using FourLines.Api.Contexts;
using FourLines.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace FourLines.Api.Seeders;

public sealed class FacilitySeeder : ISeeder
{
    public async Task SeedAsync(FourLinesContext context)
    {
        DateTimeOffset now = DateTimeOffset.UtcNow;

        var facilities = new[]
        {
            new
            {
                OwnerEmail = "admin@fourlines.local",
                Name = "Arena Central",
                Address = "Rua Principal, 100",
                City = "São Paulo",
                State = "SP",
                ZipCode = "01000000",
                RegistrationNumber = "FAC001",
                CreatedAt = now,
                UpdatedAt = now
            },
            new
            {
                OwnerEmail = "manager@fourlines.local",
                Name = "Arena Norte",
                Address = "Av. Norte, 250",
                City = "Campinas",
                State = "SP",
                ZipCode = "13000000",
                RegistrationNumber = "FAC002",
                CreatedAt = now,
                UpdatedAt = now
            },
            new
            {
                OwnerEmail = "admin@fourlines.local",
                Name = "Arena Sul",
                Address = "Rua Sul, 45",
                City = "Santos",
                State = "SP",
                ZipCode = "11000000",
                RegistrationNumber = "FAC003",
                CreatedAt = now,
                UpdatedAt = now
            }
        };

        foreach (var facility in facilities)
        {
            bool exists = await context.Facilities.AnyAsync(existingFacility => existingFacility.RegistrationNumber == facility.RegistrationNumber);

            if (exists)
            {
                continue;
            }

            User? owner = await context.Users.FirstOrDefaultAsync(existingUser => existingUser.Email == facility.OwnerEmail);

            if (owner is null)
            {
                continue;
            }

            await context.Facilities.AddAsync(new Facility
            {
                OwnerId = owner.Id,
                Name = facility.Name,
                Address = facility.Address,
                City = facility.City,
                State = facility.State,
                ZipCode = facility.ZipCode,
                RegistrationNumber = facility.RegistrationNumber,
                CreatedAt = facility.CreatedAt,
                UpdatedAt = facility.UpdatedAt
            });
        }

        await context.SaveChangesAsync();
    }
}