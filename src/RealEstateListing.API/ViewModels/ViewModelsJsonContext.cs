using System.Text.Json.Serialization;
using RealEstateListing.Domain.ValueObjects;

namespace RealEstateListing.API.ViewModels;

/// <summary>
/// JSON serialization context for API view models.
/// Uses source generators for AOT-compatible serialization.
/// </summary>
[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    PropertyNameCaseInsensitive = true,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    WriteIndented = false)]
[JsonSerializable(typeof(Money))]
[JsonSerializable(typeof(Address))]
[JsonSerializable(typeof(CurrencyCode))]
[JsonSerializable(typeof(ListingStatus))]
[JsonSerializable(typeof(CreateListingRequest))]
[JsonSerializable(typeof(UpdateListingRequest))]
[JsonSerializable(typeof(AddressRequest))]
[JsonSerializable(typeof(ListingResponse))]
[JsonSerializable(typeof(AddressResponse))]
[JsonSerializable(typeof(IEnumerable<ListingResponse>))]
[JsonSerializable(typeof(List<ListingResponse>))]
public partial class ViewModelsJsonContext : JsonSerializerContext
{
}
