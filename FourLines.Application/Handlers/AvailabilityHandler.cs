using Dapper;
using FourLines.Infrastructure.Providers;

namespace FourLines.Application.Handlers;

public class AvailabilityHandler(ConnectionProvider connectionProvider) : IAvailabilityHandler
{
    public async Task<FacilityTimeSlot> GetTimeSlots(
        Guid courtId,
        DayOfWeek selectedDayOfTheWeek,
        CancellationToken cancellationToken = default
    )
    {
        const string sql =
            @"SELECT c.id AS ""CourtId"",
                     c.maintenance_period_in_minutes AS ""MaintenancePeriodInMinutes"",
                     c.renting_period_in_minutes AS ""RentingPeriodInMinutes"",
                     c.amount AS ""PriceAmount"",
                     c.currency AS ""PriceCurrency"",
                     f.time_zone_id AS ""TimeZoneId"",
                     fs.day_of_week AS ""DayOfWeek"",
                     fs.opens_at AS ""OpensAt"",
                     fs.closes_at AS ""ClosesAt""
                FROM courts c 
               INNER JOIN facilities f ON c.facility_id = f.id 
               INNER JOIN facility_schedules fs ON fs.facility_id = f.id
               WHERE c.id = @courtId
                 AND fs.day_of_week = @selectedDayOfTheWeek";

        CommandDefinition command = new(
            sql,
            parameters: new { courtId, selectedDayOfTheWeek },
            cancellationToken: cancellationToken
        );

        using var connection = connectionProvider.Provide();

        FacilityAvailability? facilityAvailability =
            await connection.QueryFirstOrDefaultAsync<FacilityAvailability>(command);

        if (facilityAvailability is null)
            return null!;

        TimeOnly current = facilityAvailability.OpensAt;
        int reservationSlot =
            facilityAvailability.MaintenancePeriodInMinutes
            + facilityAvailability.RentingPeriodInMinutes;

        List<TimeSlot> timeSlots = [];

        while (current.AddMinutes(reservationSlot) <= facilityAvailability.ClosesAt)
        {
            TimeOnly slotEnd = current.AddMinutes(reservationSlot);

            if (slotEnd > facilityAvailability.ClosesAt)
                return null!;

            timeSlots.Add(
                new TimeSlot()
                {
                    Start = current,
                    End = slotEnd,
                    Price = new Money(
                        facilityAvailability.PriceAmount,
                        facilityAvailability.PriceCurrency
                    ),
                    Available = true,
                }
            );

            current = slotEnd;
        }

        return new FacilityTimeSlot(facilityAvailability.TimeZoneId, timeSlots);
    }
}
