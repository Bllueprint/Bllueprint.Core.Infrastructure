using Bllueprint.Core.Infrastructure.Tests.Shared;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace Bllueprint.Core.Infrastructure.Tests;

public sealed class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddBllueprintInfrastructure_RegistersIUnitOfWorkAsScoped()
    {
        var services = new ServiceCollection();

        services.AddBllueprintInfrastructure<TestDbContext>();

        ServiceDescriptor? descriptor = services.FirstOrDefault(d => d.ServiceType == typeof(IUnitOfWork));
        descriptor.Should().NotBeNull();
        descriptor!.Lifetime.Should().Be(ServiceLifetime.Scoped);
    }

    [Fact]
    public void AddBllueprintInfrastructure_ReturnsTheSameServiceCollection()
    {
        var services = new ServiceCollection();

        IServiceCollection result = services.AddBllueprintInfrastructure<TestDbContext>();

        result.Should().BeSameAs(services);
    }

    [Fact]
    public void AddRepository_RegistersDbSetAsScoped()
    {
        var services = new ServiceCollection();

        services.AddRepository<ITestRepository, ConcreteRepository, TestDbContext, TestEntity>(
            ctx => ctx.TestEntities);

        ServiceDescriptor? descriptor = services.FirstOrDefault(d => d.ServiceType == typeof(DbSet<TestEntity>));
        descriptor.Should().NotBeNull();
        descriptor!.Lifetime.Should().Be(ServiceLifetime.Scoped);
    }

    [Fact]
    public void AddRepository_RegistersInterfaceAsScoped()
    {
        var services = new ServiceCollection();

        services.AddRepository<ITestRepository, ConcreteRepository, TestDbContext, TestEntity>(
            ctx => ctx.TestEntities);

        ServiceDescriptor? descriptor = services.FirstOrDefault(d => d.ServiceType == typeof(ITestRepository));
        descriptor.Should().NotBeNull();
        descriptor!.Lifetime.Should().Be(ServiceLifetime.Scoped);
    }

    [Fact]
    public void AddRepository_RegistersCorrectImplementationType()
    {
        var services = new ServiceCollection();

        services.AddRepository<ITestRepository, ConcreteRepository, TestDbContext, TestEntity>(
            ctx => ctx.TestEntities);

        ServiceDescriptor? descriptor = services.FirstOrDefault(d => d.ServiceType == typeof(ITestRepository));
        descriptor!.ImplementationType.Should().Be(typeof(ConcreteRepository));
    }

    [Fact]
    public void AddRepository_ReturnsTheSameServiceCollection()
    {
        var services = new ServiceCollection();

        IServiceCollection result = services.AddRepository<ITestRepository, ConcreteRepository, TestDbContext, TestEntity>(
            ctx => ctx.TestEntities);

        result.Should().BeSameAs(services);
    }

    [Fact]
    public void AddRepository_ResolvesRepositoryEndToEnd()
    {
        var services = new ServiceCollection();
        DbSet<TestEntity> dbSet = Substitute.For<DbSet<TestEntity>>();
        TestDbContext ctx = Substitute.For<TestDbContext>();
        ctx.TestEntities.Returns(dbSet);
        services.AddScoped(_ => ctx);
        services.AddRepository<ITestRepository, ConcreteRepository, TestDbContext, TestEntity>(
            c => c.TestEntities);

        using ServiceProvider provider = services.BuildServiceProvider();
        using IServiceScope scope = provider.CreateScope();

        ITestRepository? repository = scope.ServiceProvider.GetService<ITestRepository>();
        repository.Should().NotBeNull().And.BeOfType<ConcreteRepository>();
    }
}
