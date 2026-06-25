using FourLines.Domain.Models;

namespace FourLines.Domain.Interfaces;

public interface IAppContext
{
    Task SaveChangesAsync();
}
