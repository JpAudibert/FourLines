using FourLines.Api.Models;

namespace FourLines.Api.Interfaces;

public interface IStandardRepository<TEntity> 
    where TEntity : BaseEntity
{
    Task AddAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task DeleteAsync(int id);
    Task<TEntity?> GetEntityAsync(int id);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task SaveChangesAsync();
}
