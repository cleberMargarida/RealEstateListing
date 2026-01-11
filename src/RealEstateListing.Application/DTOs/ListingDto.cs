using RealEstateListing.Domain.ValueObjects;

namespace RealEstateListing.Application.DTOs;

/// <summary>
/// Generated DTO for entity Listing.
/// </summary>
/// <param name="Id"> Gets or sets the Id. </param>
/// <param name="Title"> Gets or sets the Title. </param>
/// <param name="Price"> Gets or sets the Price. </param>
/// <param name="Description"> Gets or sets the Description. </param>
/// <param name="Address"> Gets or sets the Address. </param>
/// <param name="Status"> Gets or sets the Status. </param>
/// <param name="CreatedAt"> Gets or sets the CreatedAt. </param>
/// <param name="UpdatedAt"> Gets or sets the UpdatedAt. </param>
public record ListingDto(
    Guid? Id,
    string? Title,
    Money? Price,
    string? Description,
    Address? Address,
    ListingStatus? Status,
    DateTime? CreatedAt,
    DateTime? UpdatedAt);
