using Microsoft.EntityFrameworkCore;

namespace Bllueprint.Core.Infrastructure.Tests.Shared;

internal class TestDbContext : DbContext
{
    public virtual DbSet<TestEntity> TestEntities { get; set; } = null!;
}
