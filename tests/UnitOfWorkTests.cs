using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NSubstitute;

namespace Bllueprint.Core.Infrastructure.Tests;

public sealed class UnitOfWorkTests
{
    [Fact]
    public async Task CommitAsync_CallsSaveChangesAsyncOnDbContext()
    {
        DbContext context = Substitute.For<DbContext>();
        context.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(Task.FromResult(0));
        var sut = new UnitOfWork<DbContext>(context);

        await sut.CommitAsync();

        await context.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CommitAsync_PropagatesExceptionFromSaveChangesAsync()
    {
        DbContext context = Substitute.For<DbContext>();
        context.SaveChangesAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromException<int>(new DbUpdateException()));
        var sut = new UnitOfWork<DbContext>(context);

        Func<Task> act = sut.CommitAsync;

        await act.Should().ThrowAsync<DbUpdateException>();
    }
}
