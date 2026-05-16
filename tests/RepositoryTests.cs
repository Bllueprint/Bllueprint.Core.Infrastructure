using Bllueprint.Core.Infrastructure.Tests.Shared;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NSubstitute;

namespace Bllueprint.Core.Infrastructure.Tests;

public sealed class RepositoryTests
{
    private readonly DbSet<TestEntity> _dbSet;
    private readonly ConcreteRepository _sut;

    public RepositoryTests()
    {
        _dbSet = Substitute.For<DbSet<TestEntity>>();
        _sut = new ConcreteRepository(_dbSet);
    }

    [Fact]
    public async Task GetByIdAsync_WhenEntityExists_ReturnsEntity()
    {
        var entity = new TestEntity();
        _ = _dbSet.FindAsync(Arg.Any<object?[]?>()).Returns(new ValueTask<TestEntity?>(entity));

        TestEntity? result = await _sut.GetByIdAsync(entity.Id);

        result.Should().Be(entity);
    }

    [Fact]
    public async Task GetByIdAsync_WhenEntityDoesNotExist_ReturnsNull()
    {
        _dbSet.FindAsync(Arg.Any<object?[]?>()).Returns(new ValueTask<TestEntity?>(default(TestEntity)));

        TestEntity? result = await _sut.GetByIdAsync(Guid.NewGuid());

        result.Should().BeNull();
    }

    [Fact]
    public async Task AddAsync_DelegatesToDbSetAddAsync()
    {
        var entity = new TestEntity();

        await _sut.AddAsync(entity);

        await _dbSet.Received(1).AddAsync(entity, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task UpdateAsync_DelegatesToDbSetUpdate()
    {
        var entity = new TestEntity();

        await _sut.UpdateAsync(entity);

        _dbSet.Received(1).Update(entity);
    }

    [Fact]
    public async Task DeleteAsync_DelegatesToDbSetRemove()
    {
        var entity = new TestEntity();

        await _sut.DeleteAsync(entity);

        _dbSet.Received(1).Remove(entity);
    }

    [Fact]
    public void DbSet_IsExposedToSubclasses()
    {
        _sut.ExposedDbSet.Should().BeSameAs(_dbSet);
    }
}
