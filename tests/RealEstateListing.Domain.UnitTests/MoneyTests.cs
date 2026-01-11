using RealEstateListing.Domain.ValueObjects;

namespace RealEstateListing.Domain.Tests;

/// <summary>
/// Unit tests for the Money value object operations and comparisons.
/// </summary>
public class MoneyTests
{

    [Fact]
    public void FromDollar_CreatesMoneyWithUsdCurrency()
    {
        // Act
        var money = Money.FromDollar(100m);

        // Assert
        Assert.Equal(100m, money.Amount);
        Assert.Equal(CurrencyCode.USD, money.Currency);
    }

    [Fact]
    public void Zero_HasZeroAmountAndDefaultCurrency()
    {
        // Assert
        Assert.Equal(0m, Money.Zero.Amount);
        Assert.Equal((CurrencyCode)0, Money.Zero.Currency);
    }

    [Fact]
    public void Equals_TwoMoneyWithSameAmountAndCurrency_ReturnsTrue()
    {
        // Arrange
        var money1 = new Money(100m, CurrencyCode.USD);
        var money2 = new Money(100m, CurrencyCode.USD);

        // Assert
        Assert.True(money1.Equals(money2));
        Assert.True(money1 == money2);
        Assert.False(money1 != money2);
    }

    [Fact]
    public void Equals_TwoMoneyWithDifferentAmounts_ReturnsFalse()
    {
        // Arrange
        var money1 = new Money(100m, CurrencyCode.USD);
        var money2 = new Money(200m, CurrencyCode.USD);

        // Assert
        Assert.False(money1.Equals(money2));
        Assert.False(money1 == money2);
        Assert.True(money1 != money2);
    }

    [Fact]
    public void Equals_TwoMoneyWithDifferentCurrencies_ReturnsFalse()
    {
        // Arrange
        var money1 = new Money(100m, CurrencyCode.USD);
        var money2 = new Money(100m, CurrencyCode.EUR);

        // Assert
        Assert.False(money1.Equals(money2));
    }

    [Fact]
    public void Equals_WithObject_ReturnsTrueForEqualMoney()
    {
        // Arrange
        var money1 = new Money(100m, CurrencyCode.USD);
        object money2 = new Money(100m, CurrencyCode.USD);

        // Assert
        Assert.True(money1.Equals(money2));
    }

    [Fact]
    public void Equals_WithNonMoneyObject_ReturnsFalse()
    {
        // Arrange
        var money = new Money(100m, CurrencyCode.USD);

        // Assert
        Assert.False(money.Equals("not money"));
        Assert.False(money.Equals(null));
    }

    [Fact]
    public void GetHashCode_SameValuesSameHashCode()
    {
        // Arrange
        var money1 = new Money(100m, CurrencyCode.USD);
        var money2 = new Money(100m, CurrencyCode.USD);

        // Assert
        Assert.Equal(money1.GetHashCode(), money2.GetHashCode());
    }

    [Fact]
    public void CompareTo_EqualMoney_ReturnsZero()
    {
        // Arrange
        var money1 = new Money(100m, CurrencyCode.USD);
        var money2 = new Money(100m, CurrencyCode.USD);

        // Assert
        Assert.Equal(0, money1.CompareTo(money2));
    }

    [Fact]
    public void CompareTo_GreaterMoney_ReturnsPositive()
    {
        // Arrange
        var money1 = new Money(200m, CurrencyCode.USD);
        var money2 = new Money(100m, CurrencyCode.USD);

        // Assert
        Assert.True(money1.CompareTo(money2) > 0);
    }

    [Fact]
    public void CompareTo_LesserMoney_ReturnsNegative()
    {
        // Arrange
        var money1 = new Money(50m, CurrencyCode.USD);
        var money2 = new Money(100m, CurrencyCode.USD);

        // Assert
        Assert.True(money1.CompareTo(money2) < 0);
    }

    [Fact]
    public void CompareTo_DifferentCurrencies_ThrowsInvalidOperationException()
    {
        // Arrange
        var money1 = new Money(100m, CurrencyCode.USD);
        var money2 = new Money(100m, CurrencyCode.EUR);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => money1.CompareTo(money2));
    }

    [Fact]
    public void CompareTo_WithZero_ComparesAmount()
    {
        // Arrange
        var money = new Money(100m, CurrencyCode.USD);

        // Assert
        Assert.True(money.CompareTo(Money.Zero) > 0);
        Assert.True(Money.Zero.CompareTo(money) < 0);
    }

    [Fact]
    public void GreaterThan_Operator_ReturnsCorrectResult()
    {
        // Arrange
        var money1 = new Money(200m, CurrencyCode.USD);
        var money2 = new Money(100m, CurrencyCode.USD);

        // Assert
        Assert.True(money1 > money2);
        Assert.False(money2 > money1);
    }

    [Fact]
    public void GreaterThanOrEqual_Operator_ReturnsCorrectResult()
    {
        // Arrange
        var money1 = new Money(100m, CurrencyCode.USD);
        var money2 = new Money(100m, CurrencyCode.USD);
        var money3 = new Money(50m, CurrencyCode.USD);

        // Assert
        Assert.True(money1 >= money2);
        Assert.True(money1 >= money3);
        Assert.False(money3 >= money1);
    }

    [Fact]
    public void LessThan_Operator_ReturnsCorrectResult()
    {
        // Arrange
        var money1 = new Money(50m, CurrencyCode.USD);
        var money2 = new Money(100m, CurrencyCode.USD);

        // Assert
        Assert.True(money1 < money2);
        Assert.False(money2 < money1);
    }

    [Fact]
    public void LessThanOrEqual_Operator_ReturnsCorrectResult()
    {
        // Arrange
        var money1 = new Money(100m, CurrencyCode.USD);
        var money2 = new Money(100m, CurrencyCode.USD);
        var money3 = new Money(150m, CurrencyCode.USD);

        // Assert
        Assert.True(money1 <= money2);
        Assert.True(money1 <= money3);
        Assert.False(money3 <= money1);
    }

    [Fact]
    public void GreaterThan_WithZero_ComparesCorrectly()
    {
        // Assert
        Assert.True(Money.FromDollar(100m) > Money.Zero);
        Assert.False(Money.Zero > Money.FromDollar(100m));
    }

    [Fact]
    public void LessThan_WithZero_ComparesCorrectly()
    {
        // Assert
        Assert.True(Money.Zero < Money.FromDollar(100m));
        Assert.False(Money.FromDollar(100m) < Money.Zero);
    }

    [Fact]
    public void ComparisonOperators_DifferentCurrencies_ThrowsException()
    {
        // Arrange
        var usd = new Money(100m, CurrencyCode.USD);
        var eur = new Money(100m, CurrencyCode.EUR);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => usd > eur);
        Assert.Throws<InvalidOperationException>(() => usd >= eur);
        Assert.Throws<InvalidOperationException>(() => usd < eur);
        Assert.Throws<InvalidOperationException>(() => usd <= eur);
    }

    [Fact]
    public void Addition_SameCurrency_ReturnsSum()
    {
        // Arrange
        var money1 = new Money(100m, CurrencyCode.USD);
        var money2 = new Money(50m, CurrencyCode.USD);

        // Act
        var result = money1 + money2;

        // Assert
        Assert.Equal(150m, result.Amount);
        Assert.Equal(CurrencyCode.USD, result.Currency);
    }

    [Fact]
    public void Addition_WithZero_ReturnsOriginal()
    {
        // Arrange
        var money = new Money(100m, CurrencyCode.USD);

        // Assert
        Assert.Equal(money, money + Money.Zero);
        Assert.Equal(money, Money.Zero + money);
    }

    [Fact]
    public void Addition_DifferentCurrencies_ThrowsException()
    {
        // Arrange
        var usd = new Money(100m, CurrencyCode.USD);
        var eur = new Money(50m, CurrencyCode.EUR);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => usd + eur);
    }

    [Fact]
    public void Subtraction_SameCurrency_ReturnsDifference()
    {
        // Arrange
        var money1 = new Money(100m, CurrencyCode.USD);
        var money2 = new Money(30m, CurrencyCode.USD);

        // Act
        var result = money1 - money2;

        // Assert
        Assert.Equal(70m, result.Amount);
        Assert.Equal(CurrencyCode.USD, result.Currency);
    }

    [Fact]
    public void Subtraction_WithZero_ReturnsOriginal()
    {
        // Arrange
        var money = new Money(100m, CurrencyCode.USD);

        // Assert
        Assert.Equal(money, money - Money.Zero);
    }

    [Fact]
    public void Subtraction_ZeroMinusMoney_ReturnsNegative()
    {
        // Arrange
        var money = new Money(100m, CurrencyCode.USD);

        // Act
        var result = Money.Zero - money;

        // Assert
        Assert.Equal(-100m, result.Amount);
        Assert.Equal(CurrencyCode.USD, result.Currency);
    }

    [Fact]
    public void Subtraction_DifferentCurrencies_ThrowsException()
    {
        // Arrange
        var usd = new Money(100m, CurrencyCode.USD);
        var eur = new Money(50m, CurrencyCode.EUR);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => usd - eur);
    }

    [Fact]
    public void Multiplication_ReturnsProduct()
    {
        // Arrange
        var money = new Money(100m, CurrencyCode.USD);

        // Act
        var result = money * 2.5m;

        // Assert
        Assert.Equal(250m, result.Amount);
        Assert.Equal(CurrencyCode.USD, result.Currency);
    }

    [Fact]
    public void Multiplication_ByZero_ReturnsZero()
    {
        // Arrange
        var money = new Money(100m, CurrencyCode.USD);

        // Act
        var result = money * 0m;

        // Assert
        Assert.Equal(Money.Zero, result);
    }

    [Fact]
    public void Multiplication_ZeroMoney_ReturnsZero()
    {
        // Act
        var result = Money.Zero * 5m;

        // Assert
        Assert.Equal(Money.Zero, result);
    }

    [Fact]
    public void Division_ReturnsQuotient()
    {
        // Arrange
        var money = new Money(100m, CurrencyCode.USD);

        // Act
        var result = money / 4m;

        // Assert
        Assert.Equal(25m, result.Amount);
        Assert.Equal(CurrencyCode.USD, result.Currency);
    }

    [Fact]
    public void Division_ByZero_ThrowsDivideByZeroException()
    {
        // Arrange
        var money = new Money(100m, CurrencyCode.USD);

        // Act & Assert
        Assert.Throws<DivideByZeroException>(() => money / 0m);
    }

    [Fact]
    public void Division_ZeroMoney_ReturnsZero()
    {
        // Act
        var result = Money.Zero / 5m;

        // Assert
        Assert.Equal(Money.Zero, result);
    }
} 
