namespace RealEstateListing.Application;

/// <summary>
/// Base exception for application layer errors.
/// Used to communicate business operation failures with detailed context.
/// </summary>
/// <remarks>
/// Initializes a new instance of <see cref="ApplicationException"/>.
/// </remarks>
/// <param name="message">The error message describing the failure.</param>
/// <param name="errorCode">The error code categorizing this exception.</param>
/// <param name="innerException">The exception that caused this application exception.</param>
public class ApplicationException(
    string message,
    ServiceErrorCode errorCode = ServiceErrorCode.General,
    Exception? innerException = null) : Exception(message, innerException)
{
    /// <summary>
    /// Gets the error code that categorizes this exception.
    /// </summary>
    public ServiceErrorCode ErrorCode { get; } = errorCode;

    /// <summary>
    /// Creates an exception for a resource that was not found.
    /// </summary>
    /// <param name="resourceName">The name of the resource type.</param>
    /// <param name="resourceId">The identifier of the resource.</param>
    /// <returns>An <see cref="ApplicationException"/> with NotFound error code.</returns>
    public static ApplicationException NotFound(string resourceName, object resourceId)
        => new($"{resourceName} with id '{resourceId}' was not found.", ServiceErrorCode.NotFound);
}
