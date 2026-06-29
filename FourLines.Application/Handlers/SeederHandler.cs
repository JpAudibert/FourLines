using FourLines.Infrastructure.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace FourLines.Application.Handlers;

public class SeederHandler
{
    private readonly FourLinesContext _context;

    public SeederHandler(FourLinesContext context)
    {
        _context = context;
    }

    public async Task Seed()
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        using (_context)
        {
            try
            {
                await SeedRolesAsync();
                await SeedSportsAsync();
                await SeedUsers();
                await SeedFacilities();
                await SeedCourts();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }

    public async Task SeedRolesAsync()
    {
        if (!await ValidateIsEmpty<Role>())
            return;

        var roles = new List<Role>
        {
            new() { Name = "Admin" },
            new() { Name = "Player" },
            new() { Name = "Facility Owner" },
            new() { Name = "Coach" },
            new() { Name = "Manager" }
        };

        await _context.Roles.AddRangeAsync(roles);
        await _context.SaveChangesAsync();
    }

    public async Task SeedSportsAsync()
    {
        if (!await ValidateIsEmpty<Sport>())
            return;

        var sports = new List<Sport>
        {
            new() { Name = "Football", Indoor = false, StartingPlayersCount = 22, MaxPlayersCount = 26},
            new() { Name = "Futsal", Indoor = true, StartingPlayersCount = 10, MaxPlayersCount = 14 },
            new() { Name = "Basketball", Indoor = true, StartingPlayersCount = 10, MaxPlayersCount = 14 },
            new() { Name = "Volleyball", Indoor = true, StartingPlayersCount = 12, MaxPlayersCount = 16 },
            new() { Name = "Tennis", Indoor = false, StartingPlayersCount = 2, MaxPlayersCount = 4 },
            new() { Name = "Padel", Indoor = true, StartingPlayersCount = 4, MaxPlayersCount = 6 }
        };

        await _context.Sports.AddRangeAsync(sports);
        await _context.SaveChangesAsync();
    }

    public async Task SeedUsers()
    {
        if (await ValidateIsEmpty<User>() == false)
            return;

        int rolesCount = _context.Roles.Count();
        List<Role> roles = [.. _context.Roles];
        int maxUsersPerRole = 3;
        List<User> users = [];

        for (int i = 0; i < rolesCount; i++)
        {
            for (int j = 0; j < maxUsersPerRole; j++)
            {
                string password = "123456";
                User newUser = new()
                {
                    RoleId = roles[i].Id,
                    Name = $"User {i + j} {roles[i].Name}",
                    Email = $"user{i + j}_{roles[i].Name.ToLower()}@email.com",
                    Birthday = new DateOnly(1990 + i, 1, 1),
                    Phone = $"+55 11 99999-00{(i * rolesCount) + j + 1:D2}",
                    RegistrationNumber = $"USR{(i * rolesCount) + j + 1:D6}",
                    IsActive = true
                };
                newUser.PasswordHash = new PasswordHasher<User>().HashPassword(newUser, password);

                users.Add(newUser);
            }
        }

        await _context.Users.AddRangeAsync(users);
        await _context.SaveChangesAsync();
    }

    public async Task SeedFacilities()
    {
        if (!await ValidateIsEmpty<Facility>())
            return;

        int sportsCount = _context.Sports.Count();
        List<User> facilityOwners = [.. _context.Users.Where(u => u.Role.Name == "Facility Owner")];
        List<Facility> facilities = [];

        for (int i = 0; i < facilityOwners.Count; i++)
        {
            for (int j = 0; j < sportsCount; j++)
            {
                Facility facility = new()
                {
                    OwnerId = facilityOwners[i].Id,
                    Name = $"Facility {i + 1} - {facilityOwners[i].Name}",
                    Address = $"Address {i + 1} - {facilityOwners[i].Name}",
                    City = $"City {i + 1}",
                    State = $"State {i + 1}",
                    ZipCode = $"00000-00{(i * sportsCount) + j + 1:D2}",
                    RegistrationNumber = $"FAC{(i * sportsCount) + j + 1:D6}"
                };

                facilities.Add(facility);
            }
        }

        await _context.Facilities.AddRangeAsync(facilities);
        await _context.SaveChangesAsync();
    }

    public async Task SeedCourts()
    {
        if (!await ValidateIsEmpty<Court>())
            return;

        int sportsCount = _context.Sports.Count();
        List<Facility> facilities = [.. _context.Facilities];
        int maxCourtsPerSport = 3;
        List<Court> courts = [];
        for (int i = 0; i < sportsCount; i++)
        {
            for (int j = 0; j < maxCourtsPerSport; j++)
            {
                courts.Add(new Court()
                {
                    FacilityId = facilities.ElementAtOrDefault(j % facilities.Count)?.Id ?? Guid.NewGuid(),
                    SportId = _context.Sports.ElementAtOrDefault(i)?.Id ?? Guid.NewGuid(),
                    Name = $"Court {i + 1} - {facilities[i].Name}",
                    IsActive = true,
                });
            }
        }
        await _context.Courts.AddRangeAsync(courts);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ValidateIsEmpty<T>() where T : class
    {
        return await _context.Set<T>().AnyAsync() == false;
    }
}
