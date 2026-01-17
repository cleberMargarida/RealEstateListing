using RealEstateListing.Application.DTOs;
using RealEstateListing.Domain.Entities;
using RealEstateListing.Domain.Repositories;
using RealEstateListing.Domain.ValueObjects;

namespace RealEstateListing.Application.Services;

/// <summary>
/// Service implementation for listing business operations.
/// Coordinates domain logic, mapping, and persistence through Unit of Work.
/// </summary>
/// <param name="unitOfWork">The unit of work for repository access and persistence.</param>
public class ListingService(IUnitOfWork unitOfWork) : IListingService
{
    /// <inheritdoc />
    public async Task<IEnumerable<ListingDto>> GetAllAsync(CancellationToken ct = default)
    {
        var listings = await unitOfWork.Listings.GetAllAsync(ct);
        return listings.Select(l => l.ToDto());
    }

    /// <inheritdoc />
    public async Task<ListingDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var listing = await unitOfWork.Listings.GetByIdAsync(id, ct);
        return listing?.ToDto();
    }

    /// <inheritdoc />
    public async Task<ListingDto> CreateAsync(ListingDto dto, CancellationToken ct = default)
    {
        var address = dto.Address.HasValue
                ? Address.Create(dto.Address.Value.Street,
                                 dto.Address.Value.City,
                                 dto.Address.Value.State,
                                 dto.Address.Value.ZipCode)
                : default;

        var listing = Listing.Create(
            dto.Title,
            dto.Price,
            dto.Description,
            address);

        unitOfWork.Listings.Add(listing);

        await unitOfWork.Commit(ct);

        return listing.ToDto();
    }

    /// <inheritdoc />
    public async Task PublishAsync(Guid id, CancellationToken ct = default)
    {
        var listing = await unitOfWork.Listings.GetByIdAsync(id, ct)
            ?? throw ApplicationException.NotFound("Listing", id);

        listing.Publish();

        await unitOfWork.Commit(ct);
    }

    /// <inheritdoc />
    public async Task ArchiveAsync(Guid id, CancellationToken ct = default)
    {
        var listing = await unitOfWork.Listings.GetByIdAsync(id, ct)
            ?? throw ApplicationException.NotFound("Listing", id);

        listing.Archive();

        await unitOfWork.Commit(ct);
    }

    /// <inheritdoc />
    public async Task UpdateAsync(Guid id, ListingDto dto, CancellationToken ct = default)
    {
        var listing = await unitOfWork.Listings.GetByIdAsync(id, ct)
            ?? throw ApplicationException.NotFound("Listing", id);

        await unitOfWork.Commit(() =>
        {
            listing.UpdateDetails(
                dto.Title,
                dto.Price,
                dto.Description);

            if (dto.Address.HasValue)
                listing.UpdateAddress(dto.Address);

            if (listing.Status != dto.Status)
            {
                switch (dto.Status)
                {
                    case ListingStatus.Published: listing.Publish(); break;
                    case ListingStatus.Archived: listing.Archive(); break;
                    case ListingStatus.Draft: listing.Reactivate(); break;
                }
            }
        }, ct);
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var listing = await unitOfWork.Listings.GetByIdAsync(id, ct)
            ?? throw ApplicationException.NotFound("Listing", id);

        unitOfWork.Listings.Delete(listing);

        await unitOfWork.Commit(ct);
    }
}
