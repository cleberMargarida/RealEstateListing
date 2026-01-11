using RealEstateListing.Domain.Entities;

namespace RealEstateListing.Application.DTOs;

public static class ListingDtoExtensions
{
    public static ListingDto ToDto(this Listing listing) =>
        new(
            listing.Id,
            listing.Title,
            listing.Price,
            listing.Description,
            listing.Address,
            listing.Status,
            listing.CreatedAt,
            listing.UpdatedAt);
}