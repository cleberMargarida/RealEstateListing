using Microsoft.EntityFrameworkCore;
using RealEstateListing.Domain.Repositories;
using RealEstateListing.Infrastructure.Repositories;

namespace RealEstateListing.Infrastructure.Data;

/// <summary>
/// Unit of Work implementation using EF Core.
/// Coordinates repository access and transaction management through a single DbContext.
/// </summary>
/// <param name="context">The application database context.</param>
public class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
{
    /// <inheritdoc />
    /// <remarks>
    /// Repository instance is lazily created on first access.
    /// All operations share the same DbContext for transactional consistency.
    /// </remarks>
    public IListingRepository Listings => field ??= new ListingRepository(context);

    /// <inheritdoc/>
    public Task Commit() =>
          Commit(static () => Task.CompletedTask, CancellationToken.None);

    /// <inheritdoc/>
    public Task Commit(CancellationToken cancellationToken) =>
        Commit(static () => Task.CompletedTask, cancellationToken);

    /// <inheritdoc/>
    public Task Commit(Action action) =>
        Commit(() =>
        {
            action();
            return Task.CompletedTask;
        }, CancellationToken.None);

    /// <inheritdoc/>
    public Task Commit(Action action, CancellationToken cancellationToken) =>
        Commit(() =>
        {
            action();
            return Task.CompletedTask;
        }, cancellationToken);

    /// <inheritdoc/>
    public Task Commit(Func<Task> action) =>
        Commit(action, CancellationToken.None);

    /// <inheritdoc/>
    public Task Commit(Func<Task> action, CancellationToken cancellationToken) =>
        CommitInternal(
            async () =>
            {
                await action();
                return 1;
            },
            cancellationToken);

    /// <inheritdoc/>
    public Task<T> Commit<T>(Func<Task<T>> action) =>
        Commit(action, CancellationToken.None);

    /// <inheritdoc/>
    public Task<T> Commit<T>(Func<Task<T>> action, CancellationToken cancellationToken) =>
        CommitInternal(action, cancellationToken);

    private async Task<T> CommitInternal<T>(
        Func<Task<T>> action,
        CancellationToken cancellationToken)
    {
        var executionStrategy = context.Database.CreateExecutionStrategy();

        return await executionStrategy.ExecuteAsync(async () =>
        {
            await using var transaction =
                await context.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                var result = await action();

                await context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                return result;
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        });
    }
}
