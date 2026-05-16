using Bllueprint.Core.Application;
using Microsoft.EntityFrameworkCore;

namespace Bllueprint.Core.Infrastructure;

internal class UnitOfWork<TContext>(TContext context) : IUnitOfWork
where TContext : DbContext
{
    private readonly TContext _context = context;

    public Task CommitAsync() => _context.SaveChangesAsync();
}
