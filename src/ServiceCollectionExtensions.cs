using Bllueprint.Core.Application;
using Bllueprint.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Bllueprint.Core.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBllueprintInfrastructure<TContext>(this IServiceCollection services)
        where TContext : DbContext
    {
        services = AddUnitOfWork<TContext>(services);
        return services;
    }

    public static IServiceCollection AddRepository<TInterface, TImplementation, TContext, TEntity>(
        this IServiceCollection services,
        Func<TContext, DbSet<TEntity>> dbSetSelector)
        where TInterface : class, IRepository<TEntity>
        where TImplementation : class, TInterface
        where TContext : DbContext
        where TEntity : class, IAggregate
    {
        services.AddScoped(sp =>
            dbSetSelector(sp.GetRequiredService<TContext>()));

        services.AddScoped<TInterface, TImplementation>();
        return services;
    }

    private static IServiceCollection AddUnitOfWork<TContext>(this IServiceCollection services)
        where TContext : DbContext
    {
        services.AddScoped<IUnitOfWork, UnitOfWork<TContext>>();
        return services;
    }
}
