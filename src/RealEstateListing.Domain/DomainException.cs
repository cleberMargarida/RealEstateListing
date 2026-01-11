namespace RealEstateListing.Domain;

/// <summary>
/// Exception thrown when domain validation or business rules are violated.
/// Used throughout the domain layer to enforce invariants.
/// </summary>
/// <param name="message">The error message describing the domain rule violation.</param>
/// <param name="innerException">The exception that caused this domain exception.</param>
public class DomainException(string message, Exception? innerException = null) : Exception(message, innerException)
{
}
