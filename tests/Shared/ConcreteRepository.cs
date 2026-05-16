using Microsoft.EntityFrameworkCore;

namespace Bllueprint.Core.Infrastructure.Tests.Shared;

internal sealed class ConcreteRepository(DbSet<TestEntity> dbSet)
    : Repository<TestEntity>(dbSet), ITestRepository
{
    public DbSet<TestEntity> ExposedDbSet => DbSet;
}
