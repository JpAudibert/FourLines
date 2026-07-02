using FourLines.Application.DTOs.Facilities;
using FourLines.Domain.Constants;
using FourLines.Domain.Results.Facilities;

namespace FourLines.Application.Handlers;

public class FacilityHandler(FourLinesContext context)
{
    private readonly FourLinesContext _context = context;

    public async Task<Result<Facility>> Create(CreateFacilityDTO newFacility)
    {
        User? owner = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == newFacility.OwnerId && u.Role.Name == RoleConstants.FacilityOwner);
        if (owner is null)
            return Result<Facility>.Failure(FacilityCreationErrorResults.OwnerDoesNotExists);

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
}
