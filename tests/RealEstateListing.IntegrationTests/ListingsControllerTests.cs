using RealEstateListing.API.ViewModels;
using RealEstateListing.Domain.ValueObjects;
using RealEstateListing.IntegrationTests.Infrastructure;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace RealEstateListing.IntegrationTests;

/// <summary>
/// Integration tests for the ListingsController endpoints.
/// Uses SQL Server container via SqlServerContainerFixture.
/// </summary>
[Collection(nameof(IntegrationTestsCollection))]
public class ListingsControllerTests(ApiFixture factory, ITestOutputHelper outputHelper)
{
    private readonly HttpClient client = factory.SetOutputHelper(outputHelper).CreateClient();
    
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true
    };

    [Fact]
    public async Task GetAll_ReturnsSuccessStatusCode()
    {
        // Act
        var response = await client.GetAsync("/api/Listings", TestContext.Current.CancellationToken);

        // Assert
        response.EnsureSuccessStatusCode();
        var listings = await response.Content.ReadFromJsonAsync<IEnumerable<ListingResponse>>(TestContext.Current.CancellationToken);
        Assert.NotNull(listings);
    }

    [Fact]
    public async Task Create_ReturnsCreatedWithDraftStatus()
    {
        // Arrange
        var request = new CreateListingRequest(
            Title: "Test Home",
            Price: Money.FromDollar(500000m),
            Description: "A nice test property",
            Address: null);

        // Act
        var createResponse = await client.PostAsJsonAsync("/api/Listings", request, JsonOptions, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);
        var created = await createResponse.Content.ReadFromJsonAsync<ListingResponse>(JsonOptions, TestContext.Current.CancellationToken);
        Assert.NotNull(created);
        Assert.Equal(request.Title, created!.Title);
        Assert.Equal(request.Price, created.Price);
        Assert.Equal("Draft", created.Status.ToString());
    }

    [Fact]
    public async Task Create_WithInvalidTitle_ReturnsBadRequest()
    {
        // Arrange - Empty title is invalid
        var request = new CreateListingRequest(
            Title: "",
            Price: Money.FromDollar(100m),
            Description: null,
            Address: null);

        // Act
        var response = await client.PostAsJsonAsync("/api/Listings", request, JsonOptions, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Create_WithInvalidPrice_ReturnsBadRequest()
    {
        // Arrange - Zero price is invalid
        var request = new CreateListingRequest(
            Title: "Valid Title",
            Price: Money.Zero,
            Description: null,
            Address: null);

        // Act
        var response = await client.PostAsJsonAsync("/api/Listings", request, JsonOptions, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetById_WhenNotExists_ReturnsNotFound()
    {
        // Act
        var response = await client.GetAsync($"/api/Listings/{Guid.NewGuid()}", TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Delete_WhenNotExists_ReturnsNotFound()
    {
        // Act
        var response = await client.DeleteAsync($"/api/Listings/{Guid.NewGuid()}", TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
