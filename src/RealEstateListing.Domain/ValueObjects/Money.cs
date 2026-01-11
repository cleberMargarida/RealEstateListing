using System.Diagnostics;

namespace RealEstateListing.Domain.ValueObjects;

[DebuggerDisplay("{ToString()}")]
public readonly record struct Money(decimal Amount, CurrencyCode Currency) : IEquatable<Money>, IComparable<Money>
{
    public static Money Zero { get; } = new Money(0m, 0);

    public static Money FromDollar(decimal amount) => new(amount, CurrencyCode.USD);

    public override int GetHashCode()
        => Amount.GetHashCode() ^ Currency.GetHashCode();

    public bool Equals(Money other)
        => other.Currency == Currency && other.Amount == Amount;

    public int CompareTo(Money other)
    {
        if (other.Equals(Zero))
            return Amount.CompareTo(0);

        if (Equals(Zero))
            return 0m.CompareTo(other.Amount);

        if (Currency != other.Currency)
            throw new InvalidOperationException("Cannot compare Money with different currencies.");

        return Amount.CompareTo(other.Amount);
    }

    public static bool operator >(Money left, Money right)
    {
        if (right.Equals(Zero))
            return left.Amount > 0;

        if (left.Equals(Zero))
            return 0 > right.Amount;

        if (left.Currency != right.Currency)
            throw new InvalidOperationException("Cannot compare Money with different currencies.");

        return left.Amount > right.Amount;
    }

    public static bool operator >=(Money left, Money right)
    {
        if (right.Equals(Zero))
            return left.Amount >= 0;

        if (left.Equals(Zero))
            return 0 >= right.Amount;

        if (left.Currency != right.Currency)
            throw new InvalidOperationException("Cannot compare Money with different currencies.");

        return left.Amount >= right.Amount;
    }

    public static bool operator <(Money left, Money right)
    {
        if (right.Equals(Zero))
            return left.Amount < 0;

        if (left.Equals(Zero))
            return 0 < right.Amount;

        if (left.Currency != right.Currency)
            throw new InvalidOperationException("Cannot compare Money with different currencies.");

        return left.Amount < right.Amount;
    }

    public static bool operator <=(Money left, Money right)
    {
        if (right.Equals(Zero))
            return left.Amount <= 0;

        if (left.Equals(Zero))
            return 0 <= right.Amount;

        if (left.Currency != right.Currency)
            throw new InvalidOperationException("Cannot compare Money with different currencies.");

        return left.Amount <= right.Amount;
    }

    public static Money operator +(Money left, Money right)
    {
        if (right.Equals(Zero))
            return left;

        if (left.Equals(Zero))
            return right;

        if (left.Currency != right.Currency)
            throw new InvalidOperationException("Cannot add Money with different currencies.");

        return new Money(left.Amount + right.Amount, left.Currency);
    }

    public static Money operator -(Money left, Money right)
    {
        if (right.Equals(Zero))
            return left;

        if (left.Equals(Zero))
            return new Money(-right.Amount, right.Currency);

        if (left.Currency != right.Currency)
            throw new InvalidOperationException("Cannot subtract Money with different currencies.");

        return new Money(left.Amount - right.Amount, left.Currency);
    }

    public static Money operator *(Money money, decimal multiplier)
    {
        if (money.Equals(Zero) || multiplier == 0)
            return Zero;

        return new(money.Amount * multiplier, money.Currency);
    }

    public static Money operator /(Money money, decimal divisor)
    {
        if (divisor == 0)
            throw new DivideByZeroException("Cannot divide Money by zero.");

        if (money.Equals(Zero))
            return Zero;

        return new Money(money.Amount / divisor, money.Currency);
    }

    public override string ToString()
    {
        return $"{Currency} {Amount:N2}";
    }
}
