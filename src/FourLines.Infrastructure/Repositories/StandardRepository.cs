namespace FourLines.Infrastructure.Repositories;

public class StandardRepository<TEntity>(FourLinesContext context, ILogger<StandardRepository<TEntity>> logger)
    : IStandardRepository<TEntity> where TEntity : BaseEntity
{
    private readonly ILogger<StandardRepository<TEntity>> _logger = logger;
    private readonly FourLinesContext _context = context;
    private readonly DbSet<TEntity> _dbSet = context.Set<TEntity>();

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        _logger.LogInformation("Retrieving all entities of type {EntityType}", typeof(TEntity).Name);

        return await _dbSet.ToListAsync();
    }

    public async Task<TEntity?> GetEntityAsync(Guid id)
    {
        _logger.LogInformation("Retrieving entity of type {EntityType} with ID {EntityId}", typeof(TEntity).Name, id);

        return await _dbSet.FindAsync(id);
    }

    public async Task AddAsync(TEntity entity)
    {
        _logger.LogInformation("Adding new entity of type {EntityType}", typeof(TEntity).Name);

        await _dbSet.AddAsync(entity);
        await SaveChangesAsync();
    }

    public async Task UpdateAsync(TEntity entity)
    {
        _logger.LogInformation("Updating entity of type {EntityType} with ID {EntityId}", typeof(TEntity).Name, entity.Id);

        var trackedEntity = await _dbSet.FindAsync(entity.Id) ?? throw new Exception("Entity not found");

        _context.Entry(trackedEntity)
            .CurrentValues
            .SetValues(entity);

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        _logger.LogInformation("Deleting entity of type {EntityType} with ID {EntityId}", typeof(TEntity).Name, id);

        TEntity? entity = await GetEntityAsync(id);

        if (entity != null)
        {
            _dbSet.Remove(entity);
            await SaveChangesAsync();
        }
    }

    public async Task SaveChangesAsync()
    {
        _logger.LogInformation("Saving changes to the database for entity type {EntityType}", typeof(TEntity).Name);

        await _context.SaveChangesAsync();
    }
}
