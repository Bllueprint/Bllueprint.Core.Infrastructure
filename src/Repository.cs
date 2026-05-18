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

    public async Task<TEntity> AddAsync(TEntity entity)
    {
        await DbSet.AddAsync(entity);
        return entity;
    }

    public Task<TEntity> UpdateAsync(TEntity entity)
    {
        DbSet.Update(entity);
        return Task.FromResult(entity);
    }

    public Task<bool> DeleteAsync(TEntity entity)
    {
        DbSet.Remove(entity);
        return Task.FromResult(true);
    }
}
