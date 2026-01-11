namespace RealEstateListing.Domain.ValueObjects;

/// <summary>
/// Represents the lifecycle state of a listing.
/// </summary>
public enum ListingStatus
{
    /// <summary>Initial state, not publicly visible.</summary>
    Draft = 0,
    
    /// <summary>Publicly visible and active.</summary>
    Published = 1,
    
    /// <summary>No longer active, removed from public view.</summary>
    Archived = 2
}
