using RealEstateListing.Domain.Entities;
using RealEstateListing.Domain.ValueObjects;

namespace RealEstateListing.Domain.Repositories;

/// <summary>
/// Generic repository interface for entity CRUD operations.
/// Provides a standard contract for data access across aggregate roots.
/// </summary>
/// <typeparam name="T">The entity type, must be a class.</typeparam>
public interface IRepository<T> where T : class
{
    /// <summary>
    /// Retrieves all entities asynchronously.
    /// </summary>
    /// <param name="ct">Cancellation token for async operation.</param>
    /// <returns>A collection of all entities.</returns>
    Task<IEnumerable<T>> GetAllAsync(CancellationToken ct = default);
    
    /// <summary>
    /// Retrieves an entity by its unique identifier.
    /// </summary>
    /// <param name="id">The entity's unique identifier.</param>
    /// <param name="ct">Cancellation token for async operation.</param>
    /// <returns>The entity if found; otherwise, null.</returns>
    Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default);
    
    /// <summary>
    /// Adds a new entity to the repository.
    /// Changes are not persisted until SaveChangesAsync is called on the UnitOfWork.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    void Add(T entity);
    
    /// <summary>
    /// Removes an entity from the repository.
    /// Changes are not persisted until SaveChangesAsync is called on the UnitOfWork.
    /// </summary>
    /// <param name="entity">The entity to remove.</param>
    void Delete(T entity);
}

/// <summary>
/// Listing-specific repository interface extending the generic repository.
/// Provides additional query methods for Listing aggregates.
/// </summary>
public interface IListingRepository : IRepository<Listing>
{
    /// <summary>
    /// Retrieves all listings with a specific status.
    /// </summary>
    /// <param name="status">The listing status to filter by.</param>
    /// <param name="ct">Cancellation token for async operation.</param>
    /// <returns>A collection of listings matching the specified status.</returns>
    Task<IEnumerable<Listing>> GetByStatusAsync(ListingStatus status, CancellationToken ct = default);
}
