using Bllueprint.Core.Application;
using Bllueprint.Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bllueprint.Core.Infrastructure;

public abstract class Repository<TEntity>(DbSet<TEntity> dbSet) : IRepository<TEntity>
    where TEntity : class, IAggregate
{
    protected DbSet<TEntity> DbSet => dbSet;

    public async Task<TEntity?> GetByIdAsync(Guid id)
        => await DbSet.FindAsync(id);

    public async Task AddAsync(TEntity entity)
        => await DbSet.AddAsync(entity);

    public Task UpdateAsync(TEntity entity)
    {
        DbSet.Update(entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(TEntity entity)
    {
        DbSet.Remove(entity);
        return Task.CompletedTask;
    }
}
