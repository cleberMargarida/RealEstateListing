using RealEstateListing.Application.DTOs;
using RealEstateListing.Domain.ValueObjects;

namespace RealEstateListing.API.ViewModels;

/// <summary>
/// Mapperly-based mapper for converting between API models and Application DTOs.
/// Handles Request → DTO and DTO → Response transformations.
/// </summary>
public static class ListingMappingExtensions
{
    // ==================== Request to DTO ====================
    
    /// <summary>
    /// Maps a CreateListingRequest to a CreateListingDto.
    /// </summary>
    /// <param name="request">The API request model.</param>
    /// <returns>The application layer DTO.</returns>
    public static ListingDto ToDto(this CreateListingRequest request)
    {
        return new ListingDto(
            null,
            request.Title,
            request.Price,
            request.Description,
            request.Address?.MapAddressDto(), default, default, default);
    }

    /// <summary>
    /// Maps an UpdateListingRequest to an UpdateListingDto.
    /// </summary>
    /// <param name="request">The API request model.</param>
    /// <returns>The application layer DTO.</returns>
    public static ListingDto ToDto(this UpdateListingRequest request)
    {
        return new ListingDto(
            null,
            request.Title,
            request.Price,
            request.Description,
            request.Address?.MapAddressDto(),
            request.Status,
            default,
            default);
    }

    /// <summary>
    /// Maps an AddressRequest to an AddressDto.
    /// </summary>
    /// <param name="request">The API address request.</param>
    /// <returns>The application layer address DTO.</returns>
    public static Address MapAddressDto(this AddressRequest request)
    {
        return Address.Create(
            request.Street,
            request.City,
            request.State,
            request.ZipCode);
    }

    // ==================== DTO to Response ====================
    
    /// <summary>
    /// Maps a ListingDto to a ListingResponse.
    /// </summary>
    /// <param name="dto">The application layer DTO.</param>
    /// <returns>The API response model.</returns>
    public static ListingResponse ToResponse(this ListingDto dto)
    {
        return new ListingResponse(
            dto.Id!.Value,
            dto.Title!,
            dto.Price!.Value,
            dto.Description,
            dto.Address?.MapAddressResponse(),
            dto.Status!.Value,
            dto.CreatedAt!.Value,
            dto.UpdatedAt);
    }

    /// <summary>
    /// Maps an AddressDto to an AddressResponse.
    /// </summary>
    /// <param name="dto">The application layer address DTO.</param>
    /// <returns>The API address response.</returns>
    public static AddressResponse MapAddressResponse(this Address dto)
    {
        return new AddressResponse(
            dto.Street,
            dto.City,
            dto.State,
            dto.ZipCode);
    }

    /// <summary>
    /// Maps a collection of ListingDtos to ListingResponses.
    /// </summary>
    /// <param name="dtos">The DTOs to map.</param>
    /// <returns>The mapped responses.</returns>
    public static IEnumerable<ListingResponse> ToResponseList(this IEnumerable<ListingDto> dtos)
    {
        return dtos.Select(ToResponse);
    }
}
