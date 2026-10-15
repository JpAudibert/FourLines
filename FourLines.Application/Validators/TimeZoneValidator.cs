using FluentValidation;

namespace FourLines.Application.Validators;

public sealed class TimeZoneValidator : AbstractValidator<string>
{
    public TimeZoneValidator()
    {
        RuleFor(tz => tz).NotEmpty().Must(BeAValidTimeZone).WithMessage("Invalid time zone.");
    }

    private bool BeAValidTimeZone(string timeZoneId)
    {
        try
        {
            TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            return true;
        }
        catch (TimeZoneNotFoundException)
        {
            return false;
        }
        catch (InvalidTimeZoneException)
        {
            return false;
        }
    }
}