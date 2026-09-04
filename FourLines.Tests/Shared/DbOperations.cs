using FourLines.Domain.Models;
using FourLines.Infrastructure.Contexts;

namespace FourLines.Tests.Shared;

public class DbOperations
{
    public static async Task RemoveAllDataFromMemory<T>(FourLinesContext context)
        where T : BaseEntity
    {
        context.Set<T>().RemoveRange(context.Set<T>());
        await context.SaveChangesAsync();
    }

    public static async Task RemoveDataFromMemory<T>(Guid id, FourLinesContext context)
        where T : BaseEntity
    {
        T? entity = await context.Set<T>().FindAsync(id);

        if (entity != null)
        {
            context.Set<T>().Remove(entity);
            await context.SaveChangesAsync();
        }
    }

    public static async Task<T> CreateEntityInMemory<T>(T entity, FourLinesContext context)
        where T : BaseEntity
    {
        if (await context.FindAsync<T>(entity.Id) == null)
        {
            await context.Set<T>().AddAsync(entity);
            await context.SaveChangesAsync();
        }

        return entity;
    }
}
