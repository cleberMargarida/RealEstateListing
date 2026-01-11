using RealEstateListing.Application.DTOs;

namespace RealEstateListing.Application.Services;

/// <summary>
/// Service interface for listing business operations.
/// Orchestrates domain logic and coordinates with the Unit of Work.
/// </summary>
public interface IListingService
{
    /// <summary>
    /// Retrieves all listings in the system.
    /// </summary>
    /// <param name="ct">Cancellation token for async operation.</param>
    /// <returns>A collection of all listings as DTOs.</returns>
    Task<IEnumerable<ListingDto>> GetAllAsync(CancellationToken ct = default);
    
    /// <summary>
    /// Retrieves a specific listing by its unique identifier.
    /// </summary>
    /// <param name="id">The listing's unique identifier.</param>
    /// <param name="ct">Cancellation token for async operation.</param>
    /// <returns>The listing DTO if found; otherwise, null.</returns>
    Task<ListingDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    
    /// <summary>
    /// Creates a new listing in Draft status.
    /// Validates input data through domain ValueObjects.
    /// </summary>
    /// <param name="dto">The creation data for the new listing.</param>
    /// <param name="ct">Cancellation token for async operation.</param>
    /// <returns>The created listing as a DTO.</returns>
    /// <exception cref="Domain.DomainException">Thrown when validation fails.</exception>
    Task<ListingDto> CreateAsync(ListingDto dto, CancellationToken ct = default);
    
    /// <summary>
    /// Publishes a draft listing, making it publicly visible.
    /// Only listings in Draft status can be published.
    /// </summary>
    /// <param name="id">The listing's unique identifier.</param>
    /// <param name="ct">Cancellation token for async operation.</param>
    /// <exception cref="ApplicationException">Thrown with NotFound error code when listing not found.</exception>
    /// <exception cref="Domain.DomainException">Thrown when transition not allowed.</exception>
    Task PublishAsync(Guid id, CancellationToken ct = default);
    
    /// <summary>
    /// Archives a published listing, removing it from public view.
    /// Only listings in Published status can be archived.
    /// </summary>
    /// <param name="id">The listing's unique identifier.</param>
    /// <param name="ct">Cancellation token for async operation.</param>
    /// <exception cref="ApplicationException">Thrown with NotFound error code when listing not found.</exception>
    /// <exception cref="Domain.DomainException">Thrown when transition not allowed.</exception>
    Task ArchiveAsync(Guid id, CancellationToken ct = default);
    
    /// <summary>
    /// Updates the details of an existing listing.
    /// Archived listings cannot be modified.
    /// </summary>
    /// <param name="id">The listing's unique identifier.</param>
    /// <param name="dto">The updated listing data.</param>
    /// <param name="ct">Cancellation token for async operation.</param>
    /// <exception cref="ApplicationException">Thrown with NotFound error code when listing not found.</exception>
    /// <exception cref="Domain.DomainException">Thrown when modification not allowed.</exception>
    Task UpdateAsync(Guid id, ListingDto dto, CancellationToken ct = default);
    
    /// <summary>
    /// Permanently deletes a listing from the system.
    /// </summary>
    /// <param name="id">The listing's unique identifier.</param>
    /// <param name="ct">Cancellation token for async operation.</param>
    /// <exception cref="ApplicationException">Thrown with NotFound error code when listing not found.</exception>
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}
