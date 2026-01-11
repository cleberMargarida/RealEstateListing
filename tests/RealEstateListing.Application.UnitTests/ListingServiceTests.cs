using Moq;
using RealEstateListing.Application.DTOs;
using RealEstateListing.Application.Services;
using RealEstateListing.Domain;
using RealEstateListing.Domain.Entities;
using RealEstateListing.Domain.Repositories;
using RealEstateListing.Domain.ValueObjects;

namespace RealEstateListing.Application.Tests;

/// <summary>
/// Unit tests for ListingService using mocked dependencies.
/// </summary>
public class ListingServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IListingRepository> _listingRepositoryMock;
    private readonly ListingService _sut;

    public ListingServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _listingRepositoryMock = new Mock<IListingRepository>();
        
        _unitOfWorkMock.Setup(u => u.Listings).Returns(_listingRepositoryMock.Object);
        
        // Setup Commit(Action, CancellationToken) to invoke the action
        _unitOfWorkMock
            .Setup(u => u.Commit(It.IsAny<Action>(), It.IsAny<CancellationToken>()))
            .Callback<Action, CancellationToken>((action, _) => action())
            .Returns(Task.CompletedTask);
        
        _sut = new ListingService(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_CallsRepository()
    {
        // Arrange
        var listings = new List<Listing>
        {
            Listing.Create("Listing 1", Money.FromDollar(100000m)),
            Listing.Create("Listing 2", Money.FromDollar(200000m))
        };
        _listingRepositoryMock
            .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(listings);

        // Act
        await _sut.GetAllAsync();

        // Assert
        _listingRepositoryMock.Verify(r => r.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_WithCancellationToken_PassesTokenToRepository()
    {
        // Arrange
        var cts = new CancellationTokenSource();
        var token = cts.Token;
        _listingRepositoryMock
            .Setup(r => r.GetAllAsync(token))
            .ReturnsAsync(new List<Listing>());

        // Act
        await _sut.GetAllAsync(token);

        // Assert
        _listingRepositoryMock.Verify(r => r.GetAllAsync(token), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_CallsRepositoryWithCorrectId()
    {
        // Arrange
        var id = Guid.NewGuid();
        _listingRepositoryMock
            .Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Listing?)null);

        // Act
        await _sut.GetByIdAsync(id);

        // Assert
        _listingRepositoryMock.Verify(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WithValidDto_AddsListingAndSavesChanges()
    {
        // Arrange
        var dto = new ListingDto(
            null,
            "New Listing",
            Money.FromDollar(150000m),
            "A great property",
            null,
            null,
            null,
            null);

        // Act
        await _sut.CreateAsync(dto);

        // Assert
        _listingRepositoryMock.Verify(
            r => r.Add(It.Is<Listing>(l => l.Title == "New Listing")),
            Times.Once);
        _unitOfWorkMock.Verify(u => u.Commit(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WithAddress_CreatesListingWithAddress()
    {
        // Arrange
        var address = Address.Create("123 Main St", "City", "ST", "12345");
        var dto = new ListingDto(
            null,
            "New Listing",
            Money.FromDollar(150000m),
            null,
            address,
            null,
            null,
            null);

        // Act
        await _sut.CreateAsync(dto);

        // Assert
        _listingRepositoryMock.Verify(
            r => r.Add(It.Is<Listing>(l => l.Address == address)),
            Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WithInvalidTitle_ThrowsDomainException()
    {
        // Arrange
        var dto = new ListingDto(
            null,
            "",  // Invalid empty title
            Money.FromDollar(100m),
            null,
            null,
            null,
            null,
            null);

        // Act & Assert
        await Assert.ThrowsAsync<DomainException>(() => _sut.CreateAsync(dto));
        _unitOfWorkMock.Verify(u => u.Commit(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_WithNullPrice_ThrowsDomainException()
    {
        // Arrange
        var dto = new ListingDto(
            null,
            "Valid Title",
            null,  // Invalid null price
            null,
            null,
            null,
            null,
            null);

        // Act & Assert
        await Assert.ThrowsAsync<DomainException>(() => _sut.CreateAsync(dto));
        _unitOfWorkMock.Verify(u => u.Commit(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task PublishAsync_WhenListingExists_PublishesAndSaves()
    {
        // Arrange
        var id = Guid.NewGuid();
        var listing = Listing.Create("Test", Money.FromDollar(100m));
        
        _listingRepositoryMock
            .Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(listing);

        // Act
        await _sut.PublishAsync(id);

        // Assert
        Assert.Equal(ListingStatus.Published, listing.Status);
        _unitOfWorkMock.Verify(u => u.Commit(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task PublishAsync_WhenListingNotFound_ThrowsEntityNotFoundException()
    {
        // Arrange
        var id = Guid.NewGuid();
        _listingRepositoryMock
            .Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Listing?)null);

        // Act & Assert
        await Assert.ThrowsAsync<ApplicationException>(() => _sut.PublishAsync(id));
        _unitOfWorkMock.Verify(u => u.Commit(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task PublishAsync_WhenListingNotInDraft_ThrowsDomainException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var listing = Listing.Create("Test", Money.FromDollar(100m));
        listing.Publish(); // Already published
        
        _listingRepositoryMock
            .Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(listing);

        // Act & Assert
        await Assert.ThrowsAsync<DomainException>(() => _sut.PublishAsync(id));
    }

    [Fact]
    public async Task ArchiveAsync_WhenListingIsPublished_ArchivesAndSaves()
    {
        // Arrange
        var id = Guid.NewGuid();
        var listing = Listing.Create("Test", Money.FromDollar(100m));
        listing.Publish();
        
        _listingRepositoryMock
            .Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(listing);

        // Act
        await _sut.ArchiveAsync(id);

        // Assert
        Assert.Equal(ListingStatus.Archived, listing.Status);
        _unitOfWorkMock.Verify(u => u.Commit(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ArchiveAsync_WhenListingNotFound_ThrowsEntityNotFoundException()
    {
        // Arrange
        var id = Guid.NewGuid();
        _listingRepositoryMock
            .Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Listing?)null);

        // Act & Assert
        await Assert.ThrowsAsync<ApplicationException>(() => _sut.ArchiveAsync(id));
        _unitOfWorkMock.Verify(u => u.Commit(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task ArchiveAsync_WhenListingNotPublished_ThrowsDomainException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var listing = Listing.Create("Test", Money.FromDollar(100m));
        // Listing is in Draft state
        
        _listingRepositoryMock
            .Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(listing);

        // Act & Assert
        await Assert.ThrowsAsync<DomainException>(() => _sut.ArchiveAsync(id));
    }

    [Fact]
    public async Task UpdateDetailsAsync_WhenListingExists_UpdatesAndSaves()
    {
        // Arrange
        var id = Guid.NewGuid();
        var listing = Listing.Create("Original", Money.FromDollar(100m));
        var dto = new ListingDto(
            id,
            "Updated Title",
            Money.FromDollar(200m),
            "Updated description",
            null,
            null,
            null,
            null);
        
        _listingRepositoryMock
            .Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(listing);

        // Act
        await _sut.UpdateAsync(id, dto);

        // Assert
        Assert.Equal("Updated Title", listing.Title);
        Assert.Equal(Money.FromDollar(200m), listing.Price);
        _unitOfWorkMock.Verify(u => u.Commit(It.IsAny<Action>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateDetailsAsync_WhenListingNotFound_ThrowsEntityNotFoundException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var dto = new ListingDto(id, "Title", Money.FromDollar(100m), null, null, null, null, null);
        
        _listingRepositoryMock
            .Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Listing?)null);

        // Act & Assert
        await Assert.ThrowsAsync<ApplicationException>(() => _sut.UpdateAsync(id, dto));
        _unitOfWorkMock.Verify(u => u.Commit(It.IsAny<CancellationToken>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.Commit(It.IsAny<Action>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task UpdateDetailsAsync_WhenListingArchived_ThrowsDomainException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var listing = Listing.Create("Test", Money.FromDollar(100m));
        listing.Publish();
        listing.Archive();
        
        var dto = new ListingDto(id, "New Title", Money.FromDollar(200m), null, null, null, null, null);
        
        _listingRepositoryMock
            .Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(listing);

        // Act & Assert
        await Assert.ThrowsAsync<DomainException>(() => _sut.UpdateAsync(id, dto));
    }

    [Fact]
    public async Task UpdateDetailsAsync_WithNullPrice_KeepsExistingPrice()
    {
        // Arrange
        var id = Guid.NewGuid();
        var originalPrice = Money.FromDollar(100m);
        var listing = Listing.Create("Original", originalPrice);
        var dto = new ListingDto(id, "Updated", null, null, null, null, null, null);
        
        _listingRepositoryMock
            .Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(listing);

        // Act
        await _sut.UpdateAsync(id, dto);

        // Assert - null price in DTO means keep existing price
        Assert.Equal(originalPrice, listing.Price);
    }

    [Fact]
    public async Task DeleteAsync_WhenListingExists_DeletesAndSaves()
    {
        // Arrange
        var id = Guid.NewGuid();
        var listing = Listing.Create("Test", Money.FromDollar(100m));
        
        _listingRepositoryMock
            .Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(listing);

        // Act
        await _sut.DeleteAsync(id);

        // Assert
        _listingRepositoryMock.Verify(r => r.Delete(listing), Times.Once);
        _unitOfWorkMock.Verify(u => u.Commit(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WhenListingNotFound_ThrowsEntityNotFoundException()
    {
        // Arrange
        var id = Guid.NewGuid();
        _listingRepositoryMock
            .Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Listing?)null);

        // Act & Assert
        await Assert.ThrowsAsync<ApplicationException>(() => _sut.DeleteAsync(id));
        _listingRepositoryMock.Verify(r => r.Delete(It.IsAny<Listing>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.Commit(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_CanDeleteArchivedListing()
    {
        // Arrange
        var id = Guid.NewGuid();
        var listing = Listing.Create("Test", Money.FromDollar(100m));
        listing.Publish();
        listing.Archive();
        
        _listingRepositoryMock
            .Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(listing);

        // Act
        await _sut.DeleteAsync(id);

        // Assert
        _listingRepositoryMock.Verify(r => r.Delete(listing), Times.Once);
    }
}
