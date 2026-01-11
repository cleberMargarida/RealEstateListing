using RealEstateListing.Domain.Entities;
using RealEstateListing.Domain.ValueObjects;

namespace RealEstateListing.Domain.Tests;

/// <summary>
/// Unit tests for Listing creation and initial state.
/// </summary>
public class ListingCreateTests
{
    [Fact]
    public void Create_WithValidData_ReturnsListing()
    {
        // Arrange
        var title = "Beautiful House";
        var price = Money.FromDollar(250000m);
        var description = "A lovely 3-bedroom house";
        var address = Address.Create("123 Main St", "Springfield", "IL", "62701");

        // Act
        var listing = Listing.Create(title, price, description, address);

        // Assert
        Assert.NotEqual(Guid.Empty, listing.Id);
        Assert.Equal(title, listing.Title);
        Assert.Equal(price, listing.Price);
        Assert.Equal(description, listing.Description);
        Assert.Equal(address, listing.Address);
        Assert.Equal(ListingStatus.Draft, listing.Status);
        Assert.True(listing.CreatedAt <= DateTime.UtcNow);
        Assert.Null(listing.UpdatedAt);
    }

    [Fact]
    public void Create_WithMinimalData_ReturnsListing()
    {
        // Arrange
        var title = "Simple Listing";
        var price = Money.FromDollar(100m);

        // Act
        var listing = Listing.Create(title, price);

        // Assert
        Assert.NotEqual(Guid.Empty, listing.Id);
        Assert.Equal(title, listing.Title);
        Assert.Equal(price, listing.Price);
        Assert.Null(listing.Description);
        Assert.Equal(default, listing.Address);
        Assert.Equal(ListingStatus.Draft, listing.Status);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithEmptyTitle_ThrowsDomainException(string? title)
    {
        // Arrange
        var price = Money.FromDollar(100m);

        // Act & Assert
        var ex = Assert.Throws<DomainException>(() => Listing.Create(title, price));
        Assert.Contains("Title must have a value", ex.Message);
    }

    [Fact]
    public void Create_WithNullPrice_ThrowsDomainException()
    {
        // Act & Assert
        var ex = Assert.Throws<DomainException>(() => Listing.Create("Title", null));
        Assert.Contains("Price must have an monetary value", ex.Message);
    }

    [Fact]
    public void Create_WithZeroPrice_ThrowsDomainException()
    {
        // Act & Assert
        var ex = Assert.Throws<DomainException>(() => Listing.Create("Title", Money.Zero));
        Assert.Contains("Price must have an monetary value", ex.Message);
    }

    [Fact]
    public void Create_GeneratesUniqueIds()
    {
        // Act
        var listing1 = Listing.Create("Listing 1", Money.FromDollar(100m));
        var listing2 = Listing.Create("Listing 2", Money.FromDollar(200m));

        // Assert
        Assert.NotEqual(listing1.Id, listing2.Id);
    }
}
