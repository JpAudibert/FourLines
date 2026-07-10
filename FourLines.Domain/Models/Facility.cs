namespace FourLines.Domain.Models;

public class Facility : BaseEntity
{
    public Guid OwnerId { get; init; }
    public string Name { get; init; } = default!;
    public string Address { get; init; } = default!;
    public string City { get; init; } = default!;
    public string State { get; init; } = default!;
    public string ZipCode { get; init; } = default!;
    public string RegistrationNumber { get; init; } = default!;

    public User Owner { get; init; } = default!;
    public ICollection<Court> Courts { get; set; } = new List<Court>();
    public ICollection<FacilitySchedule> Schedules { get; set; } = new List<FacilitySchedule>();

    public bool IsFacilityOpened()
    {
        var currentDayOfWeek = DateTimeOffset.UtcNow.DayOfWeek;
        var currentTime = TimeOnly.FromDateTime(DateTime.Now);
        var todaySchedule = Schedules.FirstOrDefault(s => s.DayOfWeek == currentDayOfWeek);
        if (todaySchedule == null)
        {
            return false; // No schedule for today
        }
        return currentTime >= todaySchedule.OpensAt && currentTime <= todaySchedule.ClosesAt;
    }
}
