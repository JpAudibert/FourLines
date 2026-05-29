using FourLines.Api.Contexts;

namespace FourLines.Api.Seeders;

public interface ISeeder
{
    Task SeedAsync(FourLinesContext context);
}