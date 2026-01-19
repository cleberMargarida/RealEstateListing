using RealEstateListing.Domain.Entities;
using RealEstateListing.Domain.ValueObjects;

namespace RealEstateListing.Domain.Tests;

/// <summary>
/// Unit tests for Listing update operations.
/// </summary>
public class ListingUpdateTests
{

    [Fact]
    public void UpdateDetails_OnDraftListing_UpdatesAllFields()
    {
        // Arrange
        var listing = CreateDraftListing();
        var newTitle = "Updated Title";
        var newPrice = Money.FromDollar(200000m);
        var newDescription = "Updated description";

        // Act
        listing.UpdateDetails(newTitle, newPrice, newDescription);

        // Assert
        Assert.Equal(newTitle, listing.Title);
        Assert.Equal(newPrice, listing.Price);
        Assert.Equal(newDescription, listing.Description);
        Assert.NotNull(listing.UpdatedAt);
    }

    [Fact]
    public void UpdateDetails_OnPublishedListing_UpdatesAllFields()
    {
        // Arrange
        var listing = CreateDraftListing();
        listing.Publish();
        var newTitle = "Updated Title";
        var newPrice = Money.FromDollar(200000m);

        // Act
        listing.UpdateDetails(newTitle, newPrice);

        // Assert
        Assert.Equal(newTitle, listing.Title);
        Assert.Equal(newPrice, listing.Price);
    }

    [Fact]
    public void UpdateDetails_OnArchivedListing_ThrowsDomainException()
    {
        // Arrange
        var listing = CreateArchivedListing();

        // Act & Assert
        var ex = Assert.Throws<DomainException>(() =>
            listing.UpdateDetails("New Title", Money.FromDollar(100m)));
        Assert.Contains("archived", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void UpdateDetails_WithEmptyTitle_ThrowsDomainException(string? title)
    {
        // Arrange
        var listing = CreateDraftListing();

        // Act & Assert
        var ex = Assert.Throws<DomainException>(() =>
            listing.UpdateDetails(title, Money.FromDollar(100m)));
        Assert.Contains("Title cannot be empty", ex.Message);
    }

    [Fact]
    public void UpdateDetails_WithNullDescription_DoesNotClearDescription()
    {
        // Arrange
        var listing = Listing.Create(
            "Title",
            Money.FromDollar(100m),
            "Description",
            Address.Create("123 Main", "City", "ST", "12345"));

        // Act
        listing.UpdateDetails("New Title", Money.FromDollar(200m), null);

        // Assert
        Assert.NotNull(listing.Description);
    }

    [Fact]
    public void UpdateDetails_WithZeroPrice_ThrowsDomainException()
    {
        // Arrange
        var listing = CreateDraftListing();

        // Act & Assert
        var ex = Assert.Throws<DomainException>(() => listing.UpdateDetails("New Title", Money.Zero));
        Assert.Contains("Price must be a positive monetary value", ex.Message);
    }

    [Fact]
    public void UpdateDetails_WithNegativePrice_ThrowsDomainException()
    {
        // Arrange
        var listing = CreateDraftListing();

        // Act & Assert
        var ex = Assert.Throws<DomainException>(() => listing.UpdateDetails("New Title", new Money(-500m, CurrencyCode.USD)));
        Assert.Contains("Price must be a positive monetary value", ex.Message);
    }

    [Fact]
    public void UpdateAddress_OnDraftListing_UpdatesAddress()
    {
        // Arrange
        var listing = CreateDraftListing();
        var newAddress = Address.Create("789 Pine St", "Seattle", "WA", "98101");

        // Act
        listing.UpdateAddress(newAddress);

        // Assert
        Assert.Equal(newAddress, listing.Address);
        Assert.NotNull(listing.UpdatedAt);
    }

    [Fact]
    public void UpdateAddress_OnPublishedListing_UpdatesAddress()
    {
        // Arrange
        var listing = CreateDraftListing();
        listing.Publish();
        var newAddress = Address.Create("789 Pine St", "Seattle", "WA", "98101");

        // Act
        listing.UpdateAddress(newAddress);

        // Assert
        Assert.Equal(newAddress, listing.Address);
    }

    [Fact]
    public void UpdateAddress_OnArchivedListing_ThrowsDomainException()
    {
        // Arrange
        var listing = CreateArchivedListing();
        var newAddress = Address.Create("789 Pine St", "Seattle", "WA", "98101");

        // Act & Assert
        var ex = Assert.Throws<DomainException>(() => listing.UpdateAddress(newAddress));
        Assert.Contains("archived", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void UpdateAddress_WithNull_ClearsAddress()
    {
        // Arrange
        var listing = Listing.Create(
            "Title",
            Money.FromDollar(100m),
            null,
            Address.Create("123 Main", "City", "ST", "12345"));

        // Act
        listing.UpdateAddress(null);

        // Assert
        Assert.Equal(default, listing.Address);
    }

    private static Listing CreateDraftListing()
    {
        return Listing.Create(
            "Test Listing",
            Money.FromDollar(100000m),
            "Test description",
            default);
    }

    private static Listing CreateArchivedListing()
    {
        var listing = CreateDraftListing();
        listing.Publish();
        listing.Archive();
        return listing;
    }
} 
