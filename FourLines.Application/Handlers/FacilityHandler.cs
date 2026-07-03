using FourLines.Application.DTOs.Facilities;
using FourLines.Domain.Constants;
using FourLines.Domain.Results.ErrorResults;

namespace FourLines.Application.Handlers;

public class FacilityHandler(FourLinesContext context)
{
    private readonly FourLinesContext _context = context;

    public async Task<Result<Facility>> Create(CreateFacilityDTO newFacility)
    {
        User? owner = await _context.Users.FirstOrDefaultAsync(u => u.Id == newFacility.OwnerId && u.Role.Name == RoleConstants.FacilityOwner);
        if (owner is null)
            return Result<Facility>.Failure(FacilitiesErrorResults.CreateOwnerDoesNotExists);

        Facility facility = new()
        {
            Name = newFacility.Name,
            Address = newFacility.Address,
            City = newFacility.City,
            State = newFacility.State,
            ZipCode = newFacility.ZipCode,
            RegistrationNumber = newFacility.RegistrationNumber,
            OwnerId = newFacility.OwnerId,
            Owner = owner,
        };

        await _context.Facilities.AddAsync(facility);
        await _context.SaveChangesAsync();

        return Result<Facility>.Success(facility);
    }

    public async Task<Result<Facility>> Update(UpdateFacilityDTO facility)
    {
        if (facility.OwnerId == Guid.Empty)
            return Result<Facility>.Failure(FacilitiesErrorResults.UpdateEmptyOwnerId);

        int affectedRows = await _context.Facilities
                .Where(f => f.Id == facility.Id && f.OwnerId == facility.OwnerId)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(f => f.Name, facility.Name)
                    .SetProperty(f => f.Address, facility.Address)
                    .SetProperty(f => f.City, facility.City)
                    .SetProperty(f => f.State, facility.State)
                    .SetProperty(f => f.ZipCode, facility.ZipCode)
                    .SetProperty(f => f.RegistrationNumber, facility.RegistrationNumber)
                );
        if (affectedRows <= 0)
            return Result<Facility>.Failure(FacilitiesErrorResults.UpdateFacilityDoesNotExist);

        await _context.SaveChangesAsync();

        Facility? updatedFacility = await _context.Facilities.FindAsync(facility.Id);

        return Result<Facility>.Success(updatedFacility!);
    }

    public async Task<Result<bool>> Delete(Guid ownerId, Guid facilityId)
    {
        bool deleted = false;
        int facility = await _context.Facilities
            .Where(f => f.Id == facilityId && f.OwnerId == ownerId)
            .ExecuteDeleteAsync();

        if (facility <= 0)
            return Result<bool>.Failure(FacilitiesErrorResults.DeleteFacilityDoesNotExist);

        await _context.SaveChangesAsync();
        deleted = true;

        return Result<bool>.Success(deleted);
    }

    public async Task<Result<IEnumerable<Facility>>> GetFacilitiesFromOwner(Guid ownerId)
    {
        User? owner = await _context.Users.FirstOrDefaultAsync(u => u.Id == ownerId && u.Role.Name == RoleConstants.FacilityOwner);
        if (owner is null)
            return Result<IEnumerable<Facility>>.Failure(FacilitiesErrorResults.RetrieveOwnerDoesNotExists);

        IEnumerable<Facility> facilities = await _context.Facilities
            .Where(f => f.OwnerId == ownerId)
            .Select(f => new Facility
            {
                Id = f.Id,
                Name = f.Name,
                Address = f.Address,
                City = f.City,
                State = f.State,
                ZipCode = f.ZipCode,
                RegistrationNumber = f.RegistrationNumber,
                OwnerId = f.OwnerId
            })
            .ToListAsync();

        return Result<IEnumerable<Facility>>.Success(facilities);
    }

    public async Task<Result<Facility>> GetFacilityFromOwner(Guid ownerId, Guid facilityId)
    {
        User? owner = await _context.Users.FirstOrDefaultAsync(u => u.Id == ownerId && u.Role.Name == RoleConstants.FacilityOwner);
        if (owner is null)
            return Result<Facility>.Failure(FacilitiesErrorResults.RetrieveOwnerDoesNotExists);

        Facility? facility = await _context.Facilities.FirstOrDefaultAsync(f => f.Id == facilityId && f.OwnerId == ownerId);
        if (facility is null)
            return Result<Facility>.Failure(FacilitiesErrorResults.RetrieveFacilityDoesNotExist);

        return Result<Facility>.Success(facility);
    }
}
