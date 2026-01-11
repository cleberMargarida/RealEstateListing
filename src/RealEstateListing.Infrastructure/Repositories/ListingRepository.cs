using Microsoft.EntityFrameworkCore;
using RealEstateListing.Domain.Entities;
using RealEstateListing.Domain.Repositories;
using RealEstateListing.Domain.ValueObjects;
using RealEstateListing.Infrastructure.Data;

namespace RealEstateListing.Infrastructure.Repositories;

/// <summary>
/// EF Core implementation of the Listing repository.
/// Provides data access operations for Listing aggregates.
/// </summary>
/// <param name="context">The application database context.</param>
public class ListingRepository(ApplicationDbContext context) : IListingRepository
{
    /// <inheritdoc />
    public async Task<IEnumerable<Listing>> GetAllAsync(CancellationToken ct = default)
    {
        return await context.Listings
            .OrderByDescending(l => l.CreatedAt)
            .ToListAsync(ct);
    }

    /// <inheritdoc />
    public async Task<Listing?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await context.Listings.FindAsync([id], ct);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Listing>> GetByStatusAsync(ListingStatus status, CancellationToken ct = default)
    {
        return await context.Listings
            .Where(l => l.Status == status)
            .OrderByDescending(l => l.CreatedAt)
            .ToListAsync(ct);
    }

    /// <inheritdoc />
    public void Add(Listing entity)
    {
        context.Listings.Add(entity);
    }

    /// <inheritdoc />
    public void Delete(Listing entity)
    {
        context.Listings.Remove(entity);
    }
}
