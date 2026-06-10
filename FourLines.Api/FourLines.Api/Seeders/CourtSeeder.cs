using FourLines.Api.Contexts;
using FourLines.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace FourLines.Api.Seeders;

public sealed class CourtSeeder : ISeeder
{
    public async Task SeedAsync(FourLinesContext context)
    {
        DateTimeOffset now = DateTimeOffset.UtcNow;

        var courts = new[]
        {
            new
            {
                FacilityRegistrationNumber = "FAC001",
                SportName = "Futebol",
                Name = "Quadra 01",
                IsActive = true,
                CreatedAt = now,
                UpdatedAt = now
            },
            new
            {
                FacilityRegistrationNumber = "FAC001",
                SportName = "Futsal",
                Name = "Quadra 02",
                IsActive = true,
                CreatedAt = now,
                UpdatedAt = now
            },
            new
            {
                FacilityRegistrationNumber = "FAC002",
                SportName = "Volei",
                Name = "Quadra 03",
                IsActive = false,
                CreatedAt = now,
                UpdatedAt = now
            },
            new
            {
                FacilityRegistrationNumber = "FAC003",
                SportName = "Basquete",
                Name = "Quadra 04",
                IsActive = true,
                CreatedAt = now,
                UpdatedAt = now
            }
        };

        foreach (var court in courts)
        {
            bool exists = await context.Courts.AnyAsync(existingCourt =>
                existingCourt.Name == court.Name &&
                existingCourt.Facility.RegistrationNumber == court.FacilityRegistrationNumber &&
                existingCourt.Sport.Name == court.SportName);

            if (exists)
            {
                continue;
            }

            Facility? facility = await context.Facilities.FirstOrDefaultAsync(existingFacility => existingFacility.RegistrationNumber == court.FacilityRegistrationNumber);
            Sport? sport = await context.Sports.FirstOrDefaultAsync(existingSport => existingSport.Name == court.SportName);

            if (facility is null || sport is null)
            {
                continue;
            }

            await context.Courts.AddAsync(new Court
            {
                FacilityId = facility.Id,
                SportId = sport.Id,
                Name = court.Name,
                IsActive = court.IsActive,
                CreatedAt = court.CreatedAt,
                UpdatedAt = court.UpdatedAt
            });
        }

        await context.SaveChangesAsync();
    }
}