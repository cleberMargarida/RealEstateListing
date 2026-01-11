using RealEstateListing.Domain.Entities;
using RealEstateListing.Domain.ValueObjects;

namespace RealEstateListing.Domain.Tests;

/// <summary>
/// Unit tests for Listing state transitions (Draft → Published → Archived → Draft).
/// </summary>
public class ListingTransitionTests
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
        Assert.NotNull(listing.UpdatedAt);
    }

    [Fact]
    public void Publish_FromPublished_ThrowsDomainException()
    {
        // Arrange
        var listing = CreatePublishedListing();

        // Act & Assert
        var ex = Assert.Throws<DomainException>(() => listing.Publish());
        Assert.Contains("draft", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Publish_FromArchived_ThrowsDomainException()
    {
        // Arrange
        var listing = CreateArchivedListing();

        // Act & Assert
        var ex = Assert.Throws<DomainException>(() => listing.Publish());
        Assert.Contains("draft", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Archive_FromPublished_SetsStatusToArchived()
    {
        // Arrange
        var listing = CreatePublishedListing();

        // Act
        listing.Archive();

        // Assert
        Assert.Equal(ListingStatus.Archived, listing.Status);
        Assert.NotNull(listing.UpdatedAt);
    }

    [Fact]
    public void Archive_FromDraft_ThrowsDomainException()
    {
        // Arrange
        var listing = CreateDraftListing();

        // Act & Assert
        var ex = Assert.Throws<DomainException>(() => listing.Archive());
        Assert.Contains("published", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Archive_FromArchived_ThrowsDomainException()
    {
        // Arrange
        var listing = CreateArchivedListing();

        // Act & Assert
        var ex = Assert.Throws<DomainException>(() => listing.Archive());
        Assert.Contains("published", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Reactivate_FromArchived_SetsStatusToDraft()
    {
        // Arrange
        var listing = CreateArchivedListing();

        // Act
        listing.Reactivate();

        // Assert
        Assert.Equal(ListingStatus.Draft, listing.Status);
        Assert.NotNull(listing.UpdatedAt);
    }

    [Fact]
    public void Reactivate_FromDraft_ThrowsDomainException()
    {
        // Arrange
        var listing = CreateDraftListing();

        // Act & Assert
        var ex = Assert.Throws<DomainException>(() => listing.Reactivate());
        Assert.Contains("archived", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Reactivate_FromPublished_ThrowsDomainException()
    {
        // Arrange
        var listing = CreatePublishedListing();

        // Act & Assert
        var ex = Assert.Throws<DomainException>(() => listing.Reactivate());
        Assert.Contains("archived", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void FullLifecycle_DraftToPublishedToArchivedToDraft_Succeeds()
    {
        // Arrange
        var listing = CreateDraftListing();
        Assert.Equal(ListingStatus.Draft, listing.Status);

        // Act & Assert - Draft → Published
        listing.Publish();
        Assert.Equal(ListingStatus.Published, listing.Status);

        // Act & Assert - Published → Archived
        listing.Archive();
        Assert.Equal(ListingStatus.Archived, listing.Status);

        // Act & Assert - Archived → Draft
        listing.Reactivate();
        Assert.Equal(ListingStatus.Draft, listing.Status);

        // Verify can publish again
        listing.Publish();
        Assert.Equal(ListingStatus.Published, listing.Status);
    }

    private static Listing CreateDraftListing()
    {
        return Listing.Create(
            "Test Listing",
            Money.FromDollar(100000m),
            "Test description",
            default);
    }

    private static Listing CreatePublishedListing()
    {
        var listing = CreateDraftListing();
        listing.Publish();
        return listing;
    }

    private static Listing CreateArchivedListing()
    {
        var listing = CreatePublishedListing();
        listing.Archive();
        return listing;
    }
} 
