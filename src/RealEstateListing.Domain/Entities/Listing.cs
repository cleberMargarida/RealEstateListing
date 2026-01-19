using RealEstateListing.Domain.ValueObjects;

namespace RealEstateListing.Domain.Entities;

/// <summary>
/// Aggregate Root representing a real estate listing.
/// Encapsulates all business logic and enforces domain invariants.
/// State transitions: Draft → Published → Archived (with Reactivate back to Draft).
/// </summary>
public class Listing
{
    /// <summary>Gets the unique identifier.</summary>
    public Guid Id { get; private set; }

    /// <summary>Gets the listing title.</summary>
    public string Title { get; private set; }

    /// <summary>Gets the listing price.</summary>
    public Money Price { get; private set; }

    /// <summary>Gets the listing description.</summary>
    public string? Description { get; private set; }

    /// <summary>Gets the listing address, if any.</summary>
    public Address Address { get; private set; }

    /// <summary>Gets the current lifecycle status.</summary>
    public ListingStatus Status { get; private set; }

    /// <summary>Gets when the listing was created.</summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>Gets when the listing was last updated, if ever.</summary>
    public DateTime? UpdatedAt { get; private set; }

    /// <summary>
    /// Private constructor for EF Core hydration.
    /// </summary>
    private Listing() { Title = null!; }

    /// <summary>
    /// Factory method to create a new Listing in Draft status.
    /// </summary>
    /// <param name="title">The listing title (required).</param>
    /// <param name="price">The listing price (must be positive).</param>
    /// <param name="description">Optional description.</param>
    /// <param name="address">Optional address.</param>
    /// <returns>A new Listing instance in Draft status.</returns>
    public static Listing Create(string? title, Money? price, string? description = default, Address? address = default)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("Title must have a value.");

        if (!price.HasValue || price == Money.Zero || price.Value.Amount < 0m)
            throw new DomainException("Price must be a positive monetary value.");

        return new Listing
        {
            Id = Guid.NewGuid(),
            Title = title,
            Price = price.Value,
            Description = description,
            Address = address ?? default,
            Status = ListingStatus.Draft,
            CreatedAt = DateTime.UtcNow
        };
    }

    // ==================== Domain Transition Methods ====================

    /// <summary>
    /// Publishes a draft listing, making it publicly visible.
    /// </summary>
    /// <exception cref="DomainException">Thrown when listing is not in Draft status.</exception>
    public void Publish()
    {
        if (Status != ListingStatus.Draft)
            throw new DomainException("Only draft listings can be published");

        Status = ListingStatus.Published;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Archives a published listing, removing it from public view.
    /// </summary>
    /// <exception cref="DomainException">Thrown when listing is not in Published status.</exception>
    public void Archive()
    {
        if (Status != ListingStatus.Published)
            throw new DomainException("Only published listings can be archived");

        Status = ListingStatus.Archived;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Reactivates an archived listing, returning it to Draft status.
    /// </summary>
    /// <exception cref="DomainException">Thrown when listing is not in Archived status.</exception>
    public void Reactivate()
    {
        if (Status != ListingStatus.Archived)
            throw new DomainException("Only archived listings can be reactivated");

        Status = ListingStatus.Draft;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the listing details. Archived listings cannot be modified.
    /// </summary>
    /// <param name="title">The new title.</param>
    /// <param name="price">The new price (optional).</param>
    /// <param name="description">The new description (optional).</param>
    /// <exception cref="DomainException">Thrown when listing is Archived.</exception>
    public void UpdateDetails(string? title, Money? price = default, string? description = null)
    {
        if (Status == ListingStatus.Archived)
            throw new DomainException("Cannot modify archived listings");

        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("Title cannot be empty");

        Title = title;

        if (description != null)
            Description = description;

        if (price.HasValue)
        {
            if (price == Money.Zero || price.Value.Amount < 0m)
                throw new DomainException("Price must be a positive monetary value.");

            Price = price.Value;
        }

        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates only the address. Archived listings cannot be modified.
    /// </summary>
    /// <param name="address">The new address, or null to remove.</param>
    /// <exception cref="DomainException">Thrown when listing is Archived.</exception>
    public void UpdateAddress(Address? address)
    {
        if (Status == ListingStatus.Archived)
            throw new DomainException("Cannot modify archived listings");

        Address = address ?? default;
        UpdatedAt = DateTime.UtcNow;
    }
}
