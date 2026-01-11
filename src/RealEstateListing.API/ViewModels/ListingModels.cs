using RealEstateListing.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace RealEstateListing.API.ViewModels;

// ==================== Request Models ====================

/// <summary>
/// Request model for creating a new listing.
/// </summary>
/// <param name="Title">The listing title (required, max 100 characters).</param>
/// <param name="Price">The listing price (must be greater than zero).</param>
/// <param name="Description">Optional description (max 1000 characters).</param>
/// <param name="Address">Optional address information.</param>
public record CreateListingRequest(
    string Title,
    Money Price,
    string? Description,
    AddressRequest? Address);

/// <summary>
/// Request model for updating listing details (PUT semantics - replaces entire resource state).
/// Per RFC 9110, the payload represents the complete desired state of the resource.
/// </summary>
/// <param name="Title">The updated title (required, max 100 characters).</param>
/// <param name="Price">The updated price (must be greater than zero).</param>
/// <param name="Description">Optional updated description (max 1000 characters).</param>
/// <param name="Address">Optional updated address information.</param>
/// <param name="Status">The desired status (Draft, Published, Archived).</param>
public record UpdateListingRequest(
    string Title,
    Money Price,
    string? Description,
    AddressRequest? Address,
    ListingStatus? Status);

/// <summary>
/// Request model for address information.
/// </summary>
/// <param name="Street">The street address (required, max 200 characters).</param>
/// <param name="City">The city name (required, max 100 characters).</param>
/// <param name="State">Optional state or province (max 50 characters).</param>
/// <param name="ZipCode">Optional postal/zip code (max 20 characters).</param>
public record AddressRequest(
    string Street,
    string City,
    string? State,
    string? ZipCode);

// ==================== Response Models ====================

/// <summary>
/// Response model representing a listing.
/// </summary>
/// <param name="Id">The unique identifier of the listing.</param>
/// <param name="Title">The listing title.</param>
/// <param name="Price">The listing price.</param>
/// <param name="Description">The listing description, if any.</param>
/// <param name="Address">The listing address, if any.</param>
/// <param name="Status">The current status (Draft, Published, Archived).</param>
/// <param name="CreatedAt">When the listing was created.</param>
/// <param name="UpdatedAt">When the listing was last updated, if ever.</param>
public record ListingResponse(
    Guid Id,
    string Title,
    Money Price,
    string? Description,
    AddressResponse? Address,
    ListingStatus Status,
    DateTime CreatedAt,
    DateTime? UpdatedAt);

/// <summary>
/// Response model for address information.
/// </summary>
/// <param name="Street">The street address.</param>
/// <param name="City">The city name.</param>
/// <param name="State">The state or province, if provided.</param>
/// <param name="ZipCode">The postal/zip code, if provided.</param>
public record AddressResponse(
    string Street,
    string City,
    string? State,
    string? ZipCode);
