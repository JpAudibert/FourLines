using FourLines.Api.Models;

namespace FourLines.Api.Interfaces;

public interface IStandardRepository<TEntity> 
    where TEntity : BaseEntity
{
    Task AddAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task DeleteAsync(Guid id);
    Task<TEntity?> GetEntityAsync(Guid id);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task SaveChangesAsync();
}
