using FourLines.Application.DTOs.FacilitySchedules.Interfaces;

namespace FourLines.Application.Handlers;

public class FacilityScheduleHandler(FourLinesContext context) : IFacilityScheduleHandler
{
    public async Task<Result<FacilitySchedule>> Create(
        ICreateFacilityScheduleDTO newSchedule,
        CancellationToken cancellationToken = default
    )
    {
        Facility? facility = await context.Facilities.FirstOrDefaultAsync(
            f => f.Id == newSchedule.FacilityId,
            cancellationToken: cancellationToken
        );
        if (facility is null)
            return Result<FacilitySchedule>.Failure(
                FacilitySchedulesErrorResults.CreateFacilitySchedules
            );

        FacilitySchedule schedule = new()
        {
            FacilityId = newSchedule.FacilityId,
            OpensAt = newSchedule.OpensAt,
            ClosesAt = newSchedule.ClosesAt,
            DayOfWeek = newSchedule.DayOfWeek,
            Facility = facility,
        };

        await context.FacilitySchedules.AddAsync(schedule, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return Result<FacilitySchedule>.Success(schedule);
    }

    public async Task<Result<IEnumerable<FacilitySchedule>>> CreateMultiple(
        List<ICreateFacilityScheduleDTO> newSchedules,
        CancellationToken cancellationToken = default
    )
    {
        Facility? facility = await context.Facilities.FirstOrDefaultAsync(
            f => f.Id == newSchedules[0].FacilityId,
            cancellationToken: cancellationToken
        );
        if (facility is null)
            return Result<IEnumerable<FacilitySchedule>>.Failure(
                FacilitySchedulesErrorResults.CreateFacilitySchedules
            );

        List<FacilitySchedule> schedules = [];
        foreach (var schedule in newSchedules)
        {
            schedules.Add(
                new()
                {
                    FacilityId = schedule.FacilityId,
                    DayOfWeek = schedule.DayOfWeek,
                    OpensAt = schedule.OpensAt,
                    ClosesAt = schedule.ClosesAt,
                    Facility = facility,
                }
            );
        }

        await context.FacilitySchedules.AddRangeAsync(schedules, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return Result<IEnumerable<FacilitySchedule>>.Success(schedules);
    }

    public async Task<Result<FacilitySchedule>> Update(
        IUpdateFacilityScheduleDTO schedule,
        CancellationToken cancellationToken = default
    )
    {
        Facility? facility = await context.Facilities.FirstOrDefaultAsync(
            f => f.Id == schedule.FacilityId,
            cancellationToken: cancellationToken
        );
        if (facility is null)
            return Result<FacilitySchedule>.Failure(
                FacilitySchedulesErrorResults.UpdateUnknownFacility
            );

        int affectedRows = await context
            .FacilitySchedules.Where(fs =>
                fs.Id == schedule.Id && fs.FacilityId == schedule.FacilityId
            )
            .ExecuteUpdateAsync(
                fs =>
                    fs.SetProperty(fs => fs.OpensAt, schedule.OpensAt)
                        .SetProperty(fs => fs.ClosesAt, schedule.ClosesAt)
                        .SetProperty(fs => fs.DayOfWeek, schedule.DayOfWeek)
                        .SetProperty(fs => fs.UpdatedAt, DateTime.UtcNow),
                cancellationToken: cancellationToken
            );

        if (affectedRows <= 0)
            return Result<FacilitySchedule>.Failure(
                FacilitySchedulesErrorResults.UpdateUnknownSchedules
            );

        await context.SaveChangesAsync(cancellationToken);

        FacilitySchedule? updatedSchedule = await context
            .FacilitySchedules.AsNoTracking()
            .FirstOrDefaultAsync(fs => fs.Id == schedule.Id, cancellationToken: cancellationToken);

        return Result<FacilitySchedule>.Success(updatedSchedule!);
    }

    public async Task<Result<bool>> Delete(
        IDeleteFacilityScheduleDTO deleteDto,
        CancellationToken cancellationToken = default
    )
    {
        bool deleted = false;

        int affectedRows = await context
            .FacilitySchedules.Where(fs =>
                fs.Id == deleteDto.ScheduleId && fs.FacilityId == deleteDto.FacilityId
            )
            .ExecuteDeleteAsync(cancellationToken: cancellationToken);

        if (affectedRows <= 0)
            return Result<bool>.Failure(FacilitySchedulesErrorResults.DeleteUnknownSchedules);

        await context.SaveChangesAsync(cancellationToken);
        deleted = true;

        return Result<bool>.Success(deleted);
    }

    public async Task<Result<IEnumerable<FacilitySchedule>>> GetSchedules(
        Guid facilityId,
        CancellationToken cancellationToken = default
    )
    {
        IEnumerable<FacilitySchedule>? schedules = await context
            .FacilitySchedules.Where(fs => fs.FacilityId == facilityId)
            .AsNoTracking()
            .Select(fs => new FacilitySchedule()
            {
                Id = fs.Id,
                FacilityId = facilityId,
                OpensAt = fs.OpensAt,
                ClosesAt = fs.ClosesAt,
                DayOfWeek = fs.DayOfWeek,
                CreatedAt = fs.CreatedAt,
                UpdatedAt = fs.UpdatedAt,
            })
            .ToListAsync(cancellationToken: cancellationToken);

        if (!schedules.Any() || schedules is null)
            return Result<IEnumerable<FacilitySchedule>>.Failure(
                FacilitySchedulesErrorResults.RetrieveFacilitySchedules
            );

        return Result<IEnumerable<FacilitySchedule>>.Success(schedules);
    }
}
