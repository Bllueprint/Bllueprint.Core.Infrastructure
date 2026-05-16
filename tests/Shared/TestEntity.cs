using Bllueprint.Core.Domain;

namespace Bllueprint.Core.Infrastructure.Tests.Shared;

internal sealed class TestEntity : IAggregate
{
    public Guid Id { get; init; } = Guid.NewGuid();
}
