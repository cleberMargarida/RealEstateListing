namespace RealEstateListing.Application;

/// <summary>
/// Defines error codes for application service operations.
/// Used to categorize errors and map them to appropriate HTTP status codes.
/// </summary>
public enum ServiceErrorCode
{
    /// <summary>
    /// General error without specific categorization.
    /// Maps to HTTP 400 Bad Request.
    /// </summary>
    General = 0,

    /// <summary>
    /// The requested resource was not found.
    /// Maps to HTTP 404 Not Found.
    /// </summary>
    NotFound = 1,

    /// <summary>
    /// The operation is not allowed due to business rules.
    /// Maps to HTTP 400 Bad Request.
    /// </summary>
    InvalidOperation = 2,

    /// <summary>
    /// A conflict occurred (e.g., duplicate resource).
    /// Maps to HTTP 409 Conflict.
    /// </summary>
    Conflict = 3,

    /// <summary>
    /// Validation of input data failed.
    /// Maps to HTTP 422 Unprocessable Entity.
    /// </summary>
    ValidationFailed = 4
}
