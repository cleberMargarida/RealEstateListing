
namespace RealEstateListing.Domain.Repositories;

/// <summary>
/// Unit of Work interface that coordinates repository operations and transaction management.
/// Provides access to repositories through a single point, ensuring transactional consistency.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Gets the Listing repository for accessing listing aggregates.
    /// All repository operations are tracked and persisted via SaveChangesAsync.
    /// </summary>
    IListingRepository Listings { get; }

    /// <summary>  
    /// Commits a set of changes within a unit of work using a synchronous action.  
    /// </summary>  
    /// <param name="action">The synchronous action to execute within the unit of work.</param>  
    /// <returns>A task representing the asynchronous operation.</returns>  
    /// <remarks>  
    /// Use this method to ensure atomicity when performing multiple operations synchronously.  
    /// </remarks>  
    Task Commit(Action action);

    /// <summary>  
    /// Commits a set of changes within a unit of work using a synchronous action, with support for cancellation.  
    /// </summary>  
    /// <param name="action">The synchronous action to execute within the unit of work.</param>  
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>  
    /// <returns>A task representing the asynchronous operation.</returns>  
    /// <remarks>  
    /// Use this method to ensure atomicity when performing multiple operations synchronously, with the ability to cancel the operation.  
    /// </remarks>  
    Task Commit(Action action, CancellationToken cancellationToken);

    /// <summary>  
    /// Commits a set of changes within a unit of work using an asynchronous action.  
    /// </summary>  
    /// <param name="asyncAction">The asynchronous action to execute within the unit of work.</param>  
    /// <returns>A task representing the asynchronous operation.</returns>  
    /// <remarks>  
    /// Use this method to ensure atomicity when performing multiple operations asynchronously.  
    /// </remarks>  
    Task Commit(Func<Task> asyncAction);

    /// <summary>  
    /// Commits a set of changes within a unit of work using an asynchronous action, with support for cancellation.  
    /// </summary>  
    /// <param name="asyncAction">The asynchronous action to execute within the unit of work.</param>  
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>  
    /// <returns>A task representing the asynchronous operation.</returns>  
    /// <remarks>  
    /// Use this method to ensure atomicity when performing multiple operations asynchronously, with the ability to cancel the operation.  
    /// </remarks>  
    Task Commit(Func<Task> asyncAction, CancellationToken cancellationToken);

    /// <summary>  
    /// Commits a set of changes within a unit of work without any specific action.  
    /// </summary>  
    /// <returns>A task representing the asynchronous operation.</returns>  
    /// <remarks>  
    /// Use this method to commit changes without requiring a specific action, typically for scenarios where distributed lock guarantees are not needed.  
    /// </remarks>  
    Task Commit();

    /// <summary>  
    /// Commits a set of changes within a unit of work without any specific action, with support for cancellation.  
    /// </summary>  
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>  
    /// <returns>A task representing the asynchronous operation.</returns>  
    /// <remarks>  
    /// Use this method to commit changes without requiring a specific action, with the ability to cancel the operation, typically for scenarios where distributed lock guarantees are not needed.  
    /// </remarks>  
    Task Commit(CancellationToken cancellationToken);
}
