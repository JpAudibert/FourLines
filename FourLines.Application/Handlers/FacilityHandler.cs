namespace FourLines.Application.Handlers;

public class FacilityHandler(FourLinesContext context) : IFacilityHandler
{
    private readonly FourLinesContext _context = context;

    public async Task<Result<Facility>> Create(CreateFacilityDTO createDto)
    {
        User? owner = await _context.Users.FirstOrDefaultAsync(u =>
            u.Id == createDto.OwnerId && u.Role.Name == RoleConstants.FacilityOwner
        );
        if (owner is null)
            return Result<Facility>.Failure(FacilitiesErrorResults.CreateOwnerDoesNotExists);

        Facility facility = new()
        {
            Name = createDto.Name,
            Address = createDto.Address,
            City = createDto.City,
            State = createDto.State,
            ZipCode = createDto.ZipCode,
            RegistrationNumber = createDto.RegistrationNumber,
            OwnerId = createDto.OwnerId,
            Owner = owner,
        };

        await _context.Facilities.AddAsync(facility);
        await _context.SaveChangesAsync();

        return Result<Facility>.Success(facility);
    }

    public async Task<Result<bool>> Delete(DeleteFacilityDTO deleteDto)
    {
        bool deleted = false;
        int facility = await _context
            .Facilities.Where(f => f.Id == deleteDto.FacilityId && f.OwnerId == deleteDto.OwnerId)
            .ExecuteDeleteAsync();

        if (facility <= 0)
            return Result<bool>.Failure(FacilitiesErrorResults.DeleteFacilityDoesNotExist);

        await _context.SaveChangesAsync();
        deleted = true;

        return Result<bool>.Success(deleted);
    }

    public async Task<Result<IEnumerable<Facility>>> GetFacilitiesFromOwner(Guid ownerId)
    {
        User? owner = await _context.Users.FirstOrDefaultAsync(u =>
            u.Id == ownerId && u.Role.Name == RoleConstants.FacilityOwner
        );
        if (owner is null)
            return Result<IEnumerable<Facility>>.Failure(
                FacilitiesErrorResults.RetrieveOwnerDoesNotExists
            );

        IEnumerable<Facility> facilities = await _context
            .Facilities.Where(f => f.OwnerId == ownerId)
            .Select(f => new Facility
            {
                Id = f.Id,
                Name = f.Name,
                Address = f.Address,
                City = f.City,
                State = f.State,
                ZipCode = f.ZipCode,
                RegistrationNumber = f.RegistrationNumber,
                OwnerId = f.OwnerId,
            })
            .ToListAsync();

        return Result<IEnumerable<Facility>>.Success(facilities);
    }

    public async Task<Result<Facility>> GetFacilityFromOwner(Guid ownerId, Guid facilityId)
    {
        User? owner = await _context.Users.FirstOrDefaultAsync(u =>
            u.Id == ownerId && u.Role.Name == RoleConstants.FacilityOwner
        );
        if (owner is null)
            return Result<Facility>.Failure(FacilitiesErrorResults.RetrieveOwnerDoesNotExists);

        Facility? facility = await _context.Facilities.FirstOrDefaultAsync(f =>
            f.Id == facilityId && f.OwnerId == ownerId
        );
        if (facility is null)
            return Result<Facility>.Failure(FacilitiesErrorResults.RetrieveFacilityDoesNotExist);

        return Result<Facility>.Success(facility);
    }

    public async Task<Result<IEnumerable<Facility>>> GetAllFacilities()
    {
        IEnumerable<Facility> facilities = await _context
            .Facilities.Select(f => new Facility
            {
                Id = f.Id,
                Name = f.Name,
                Address = f.Address,
                City = f.City,
                State = f.State,
                ZipCode = f.ZipCode,
                RegistrationNumber = f.RegistrationNumber,
                OwnerId = f.OwnerId,
            })
            .ToListAsync();

        if (!facilities.Any())
            return Result<IEnumerable<Facility>>.Failure(
                FacilitiesErrorResults.RetrieveNoFacilities
            );

        return Result<IEnumerable<Facility>>.Success(facilities);
    }

    public async Task<Result<Facility>> Update(UpdateFacilityDTO updateDto)
    {
        if (updateDto.OwnerId == Guid.Empty)
            return Result<Facility>.Failure(FacilitiesErrorResults.UpdateEmptyOwnerId);

        int affectedRows = await _context
            .Facilities.Where(f => f.Id == updateDto.Id && f.OwnerId == updateDto.OwnerId)
            .ExecuteUpdateAsync(setters =>
                setters
                    .SetProperty(f => f.Name, updateDto.Name)
                    .SetProperty(f => f.Address, updateDto.Address)
                    .SetProperty(f => f.City, updateDto.City)
                    .SetProperty(f => f.State, updateDto.State)
                    .SetProperty(f => f.ZipCode, updateDto.ZipCode)
                    .SetProperty(f => f.RegistrationNumber, updateDto.RegistrationNumber)
            );
        if (affectedRows <= 0)
            return Result<Facility>.Failure(FacilitiesErrorResults.UpdateFacilityDoesNotExist);

        await _context.SaveChangesAsync();

        Facility? updatedFacility = await _context
            .Facilities.AsNoTracking()
            .FirstOrDefaultAsync(f => f.Id == updateDto.Id);

        return Result<Facility>.Success(updatedFacility!);
    }
}
