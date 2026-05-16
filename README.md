# Bllueprint.Core.Infrastructure

A lightweight EF Core infrastructure layer for DDD applications. Provides a generic repository pattern and a unit of work, wired up through a single DI registration call.

## Installation

```bash
dotnet add package Bllueprint.Core.Infrastructure
```

## What it does

The package provides four things:

**`IRepository<TEntity>`** — generic repository contract for domain aggregates, covering the four core persistence operations.

**`Repository<TEntity>`** — abstract EF Core implementation of `IRepository<TEntity>`. Accepts a `DbSet<TEntity>` via primary constructor and delegates directly to EF Core. Inherit from it to create a concrete repository for your aggregate.

**`IUnitOfWork`** — single-method contract for committing a unit of work.

**`UnitOfWork<TContext>`** — EF Core implementation that calls `SaveChangesAsync` on your `DbContext`.

## Usage

### 1. Register the infrastructure

Call `AddBllueprintInfrastructure` once per `DbContext` to register `IUnitOfWork`:

```csharp
services.AddBllueprintInfrastructure<AppDbContext>();
```

### 2. Create and register a repository

Inherit `Repository<TEntity>` and declare your interface:

```csharp
public interface IOrderRepository : IRepository<Order> { }

public class OrderRepository(DbSet<Order> dbSet)
    : Repository<Order>(dbSet), IOrderRepository { }
```

Register it with `AddRepository`, passing a selector that picks the right `DbSet` from your context:

```csharp
services.AddRepository<IOrderRepository, OrderRepository, AppDbContext, Order>(
    ctx => ctx.Orders);
```

Both `IUnitOfWork` and `IOrderRepository` are registered as **Scoped**.

### 3. Use in your application

```csharp
public class CreateOrderHandler(IOrderRepository repository, IUnitOfWork unitOfWork)
{
    public async Task Handle(CreateOrderCommand command)
    {
        var order = new Order(command.Id, command.CustomerId);
        await repository.AddAsync(order);
        await unitOfWork.CommitAsync();
    }
}
```

## IRepository contract

| Method | Description |
|---|---|
| `GetByIdAsync(Guid id)` | Finds by primary key; returns `null` if not found |
| `AddAsync(TEntity entity)` | Stages the entity for insert |
| `UpdateAsync(TEntity entity)` | Stages the entity for update |
| `DeleteAsync(TEntity entity)` | Stages the entity for delete |

Changes are not persisted until `IUnitOfWork.CommitAsync()` is called.

## Requirements

- .NET 10 or later
- `Microsoft.EntityFrameworkCore`
- Entities must implement `IAggregate` from `Bllueprint.Core.Domain`

## Related packages

| Package | Role |
|---|---|
| `Bllueprint.Core.Domain` | Defines `IAggregate` and domain primitives |
| `Bllueprint.Core.Application` | Application layer contracts (`ICommandResult<T>`, etc.) |
| `MediatR` | Command dispatching |
