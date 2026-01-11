using System.Diagnostics;

namespace RealEstateListing.Domain.ValueObjects;

/// <summary>
/// Value Object representing a physical address.
/// Immutable with required street and city, optional state and zip code.
/// </summary>
[DebuggerDisplay("{ToString()}}")]
public readonly record struct Address
{
    /// <summary>Gets the street address.</summary>
    public string Street { get; }
    
    /// <summary>Gets the city name.</summary>
    public string City { get; }
    
    /// <summary>Gets the state or province, if provided.</summary>
    public string? State { get; }
    
    /// <summary>Gets the postal/zip code, if provided.</summary>
    public string? ZipCode { get; }
    
    private Address(string street, string city, string? state, string? zipCode)
    {
        Street = street;
        City = city;
        State = state;
        ZipCode = zipCode;
    }
    
    /// <summary>
    /// Creates a new Address instance with validation.
    /// </summary>
    /// <param name="street">The street address (required, max 200 chars).</param>
    /// <param name="city">The city name (required, max 100 chars).</param>
    /// <param name="state">Optional state or province.</param>
    /// <param name="zipCode">Optional postal/zip code.</param>
    /// <returns>A validated Address instance.</returns>
    /// <exception cref="DomainException">Thrown when street or city validation fails.</exception>
    public static Address Create(string street, string city, string? state, string? zipCode)
    {
        if (string.IsNullOrWhiteSpace(street))
            throw new DomainException("Street is required");
            
        if (street.Length > 200)
            throw new DomainException("Street cannot exceed 200 characters");
            
        if (string.IsNullOrWhiteSpace(city))
            throw new DomainException("City is required");
            
        if (city.Length > 100)
            throw new DomainException("City cannot exceed 100 characters");
            
        return new Address(street.Trim(), city.Trim(), state?.Trim(), zipCode?.Trim());
    }
    
    /// <inheritdoc />
    public override string ToString() => 
        string.IsNullOrEmpty(State) 
            ? $"{Street}, {City}" 
            : $"{Street}, {City}, {State} {ZipCode}".Trim();
}
