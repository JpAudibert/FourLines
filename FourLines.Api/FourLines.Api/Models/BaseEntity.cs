namespace FourLines.Api.Models;

public abstract class BaseEntity
{
    public Guid Id { get; init; } =  Guid.NewGuid();
    public DateTimeOffset CreatedAt { get; init; }  = DateTimeOffset.Now;
    public DateTimeOffset UpdatedAt { get; init; }   = DateTimeOffset.Now;
}
