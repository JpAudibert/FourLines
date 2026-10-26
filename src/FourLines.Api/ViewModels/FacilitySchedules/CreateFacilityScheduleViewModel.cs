namespace FourLines.Api.ViewModels.FacilitySchedules;

public class CreateFacilityScheduleViewModel
{
    [Required(ErrorMessage = "Day of the week is required")]
    public DayOfWeek DayOfWeek { get; init; }

    [Required(ErrorMessage = "Opening time is required")]
    public TimeOnly OpensAt { get; init; }

    [Required(ErrorMessage = "Closing time is required")]
    public TimeOnly ClosesAt { get; init; }
}
