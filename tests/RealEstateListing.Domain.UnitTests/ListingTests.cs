using RealEstateListing.Domain.Entities;
using RealEstateListing.Domain.ValueObjects;

namespace RealEstateListing.Domain.Tests;

public class ListingTests
{
    [Fact]
    public void Publish_FromDraft_SetsStatusToPublished()
    {
        // Arrange
        var listing = CreateDraftListing();

        // Act
        listing.Publish();

        // Assert
        Assert.Equal(ListingStatus.Published, listing.Status);
    }

    [Fact]
    public void Publish_FromPublished_ThrowsDomainException()
    {
        // Arrange
        var listing = CreateDraftListing();
        listing.Publish();

        // Act & Assert
        var ex = Assert.Throws<DomainException>(() => listing.Publish());
        Assert.Contains("draft", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Archive_FromPublished_SetsStatusToArchived()
    {
        // Arrange
        var listing = CreateDraftListing();
        listing.Publish();

        // Act
        listing.Archive();

        // Assert
        Assert.Equal(ListingStatus.Archived, listing.Status);
    }

    [Fact]
    public void UpdateDetails_OnArchivedListing_ThrowsDomainException()
    {
        // Arrange
        var listing = CreateDraftListing();
        listing.Publish();
        listing.Archive();

        // Act & Assert
        var ex = Assert.Throws<DomainException>(() =>
            listing.UpdateDetails("New Title", Money.FromDollar(100m)));

        Assert.Contains("archived", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    private static Listing CreateDraftListing()
    {
        return Listing.Create(
            "Test Listing",
            Money.FromDollar(100000m),
            "Test description",
            default);
    }
}
