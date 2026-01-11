using RealEstateListing.Domain;
using RealEstateListing.Domain.ValueObjects;

namespace RealEstateListing.Domain.Tests;

/// <summary>
/// Unit tests for the Address value object validation and behavior.
/// </summary>
public class AddressTests
{
    [Fact]
    public void Create_WithValidData_ReturnsAddress()
    {
        // Act
        var address = Address.Create("123 Main St", "New York", "NY", "10001");

        // Assert
        Assert.Equal("123 Main St", address.Street);
        Assert.Equal("New York", address.City);
        Assert.Equal("NY", address.State);
        Assert.Equal("10001", address.ZipCode);
    }

    [Fact]
    public void Create_WithNullState_ReturnsAddressWithNullState()
    {
        // Act
        var address = Address.Create("123 Main St", "New York", null, "10001");

        // Assert
        Assert.Equal("123 Main St", address.Street);
        Assert.Equal("New York", address.City);
        Assert.Null(address.State);
        Assert.Equal("10001", address.ZipCode);
    }

    [Fact]
    public void Create_WithNullZipCode_ReturnsAddressWithNullZipCode()
    {
        // Act
        var address = Address.Create("123 Main St", "New York", "NY", null);

        // Assert
        Assert.Equal("123 Main St", address.Street);
        Assert.Equal("New York", address.City);
        Assert.Equal("NY", address.State);
        Assert.Null(address.ZipCode);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithEmptyStreet_ThrowsDomainException(string? street)
    {
        // Act & Assert
        var ex = Assert.Throws<DomainException>(() => 
            Address.Create(street!, "New York", "NY", "10001"));
        Assert.Contains("Street is required", ex.Message);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithEmptyCity_ThrowsDomainException(string? city)
    {
        // Act & Assert
        var ex = Assert.Throws<DomainException>(() => 
            Address.Create("123 Main St", city!, "NY", "10001"));
        Assert.Contains("City is required", ex.Message);
    }

    [Fact]
    public void Create_WithStreetExceeding200Characters_ThrowsDomainException()
    {
        // Arrange
        var longStreet = new string('a', 201);

        // Act & Assert
        var ex = Assert.Throws<DomainException>(() => 
            Address.Create(longStreet, "New York", "NY", "10001"));
        Assert.Contains("Street cannot exceed 200 characters", ex.Message);
    }

    [Fact]
    public void Create_WithCityExceeding100Characters_ThrowsDomainException()
    {
        // Arrange
        var longCity = new string('a', 101);

        // Act & Assert
        var ex = Assert.Throws<DomainException>(() => 
            Address.Create("123 Main St", longCity, "NY", "10001"));
        Assert.Contains("City cannot exceed 100 characters", ex.Message);
    }

    [Fact]
    public void Create_TrimsWhitespace_FromAllFields()
    {
        // Act
        var address = Address.Create("  123 Main St  ", "  New York  ", "  NY  ", "  10001  ");

        // Assert
        Assert.Equal("123 Main St", address.Street);
        Assert.Equal("New York", address.City);
        Assert.Equal("NY", address.State);
        Assert.Equal("10001", address.ZipCode);
    }

    [Fact]
    public void ToString_WithState_ReturnsFullAddress()
    {
        // Arrange
        var address = Address.Create("123 Main St", "New York", "NY", "10001");

        // Act
        var result = address.ToString();

        // Assert
        Assert.Equal("123 Main St, New York, NY 10001", result);
    }

    [Fact]
    public void ToString_WithoutState_ReturnsStreetAndCity()
    {
        // Arrange
        var address = Address.Create("123 Main St", "New York", null, null);

        // Act
        var result = address.ToString();

        // Assert
        Assert.Equal("123 Main St, New York", result);
    }

    [Fact]
    public void Equality_TwoAddressesWithSameValues_AreEqual()
    {
        // Arrange
        var address1 = Address.Create("123 Main St", "New York", "NY", "10001");
        var address2 = Address.Create("123 Main St", "New York", "NY", "10001");

        // Assert
        Assert.Equal(address1, address2);
    }

    [Fact]
    public void Equality_TwoAddressesWithDifferentValues_AreNotEqual()
    {
        // Arrange
        var address1 = Address.Create("123 Main St", "New York", "NY", "10001");
        var address2 = Address.Create("456 Oak Ave", "New York", "NY", "10001");

        // Assert
        Assert.NotEqual(address1, address2);
    }
}
