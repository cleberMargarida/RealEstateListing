using System.Linq;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using RealEstateListing.API.ViewModels;

namespace RealEstateListing.API.Swagger
{
    /// <summary>
    /// Adds detailed response descriptions and examples for common status codes used by the API.
    /// - 200 / 201 -> `ListingResponse` or `IEnumerable<ListingResponse>` examples
    /// - 204 -> descriptive "No Content"
    /// - 400 -> `ValidationProblemDetails` example
    /// - 404 -> `ProblemDetails` example
    /// </summary>
    internal class SwaggerResponsesOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Responses == null || operation.Responses.Count == 0)
                return;

            foreach (var (status, response) in operation.Responses.ToList())
            {
                // ensure content dictionary
                if (response.Content == null || !response.Content.ContainsKey("application/json"))
                    response.Content["application/json"] = new OpenApiMediaType();

                var media = response.Content["application/json"];

                switch (status)
                {
                    case "200":
                    case "201":
                        ApplySuccessExample(context, operation, media);
                        response.Description ??= status == "201" ? "Created" : "OK";
                        break;

                    case "204":
                        response.Description = "No content.";
                        // no body for 204
                        response.Content.Remove("application/json");
                        break;

                    case "400":
                        response.Description ??= "Bad request — validation failed.";
                        media.Schema = media.Schema ?? new OpenApiSchema { Type = "object" };
                        media.Example = ValidationProblemDetailsExample();
                        break;

                    case "404":
                        response.Description ??= "Not found.";
                        media.Schema = media.Schema ?? new OpenApiSchema { Type = "object" };
                        media.Example = NotFoundProblemDetailsExample();
                        break;

                    default:
                        // leave others as-is
                        break;
                }
            }
        }

        private static void ApplySuccessExample(OperationFilterContext context, OpenApiOperation operation, OpenApiMediaType media)
        {
            // If operation returns a collection of ListingResponse, give an array example
            var returnsEnumerable = operation.Responses.Values
                .SelectMany(r => r.Content?.Values ?? Enumerable.Empty<OpenApiMediaType>())
                .Any(m => m.Schema != null && m.Schema.Type == "array");

            if (returnsEnumerable)
            {
                media.Example = new OpenApiArray
                {
                    ExampleListing(),
                };
                return;
            }

            // Default single ListingResponse example (if schema references ListingResponse)
            media.Example = ExampleListing();
        }

        private static OpenApiObject ExampleListing()
            => new OpenApiObject
            {
                ["id"] = new OpenApiString("f47ac10b-58cc-4372-a567-0e02b2c3d479"),
                ["title"] = new OpenApiString("Light-filled 3‑bed near downtown"),
                ["price"] = new OpenApiObject
                {
                    ["amount"] = new OpenApiDouble(350000.00),
                    ["currency"] = new OpenApiInteger((int)RealEstateListing.Domain.ValueObjects.CurrencyCode.USD)
                },
                ["description"] = new OpenApiString("Recently renovated, open-plan living, 2 baths, garage."),
                ["address"] = new OpenApiObject
                {
                    ["street"] = new OpenApiString("123 Maple Ave"),
                    ["city"] = new OpenApiString("Springfield"),
                    ["state"] = new OpenApiString("IL"),
                    ["zipCode"] = new OpenApiString("62704")
                },
                ["status"] = new OpenApiInteger((int)RealEstateListing.Domain.ValueObjects.ListingStatus.Published),
                ["createdAt"] = new OpenApiString(DateTime.UtcNow.ToString("o")),
                ["updatedAt"] = new OpenApiString(DateTime.UtcNow.ToString("o"))
            };

        private static OpenApiObject ValidationProblemDetailsExample()
            => new OpenApiObject
            {
                ["type"] = new OpenApiString("https://tools.ietf.org/html/rfc7231#section-6.5.1"),
                ["title"] = new OpenApiString("One or more validation errors occurred."),
                ["status"] = new OpenApiInteger(400),
                ["traceId"] = new OpenApiString("|abc123def456."),
                ["errors"] = new OpenApiObject
                {
                    ["title"] = new OpenApiArray { new OpenApiString("The Title field is required.") }
                }
            };

        private static OpenApiObject NotFoundProblemDetailsExample()
            => new OpenApiObject
            {
                ["type"] = new OpenApiString("https://tools.ietf.org/html/rfc7231#section-6.5.4"),
                ["title"] = new OpenApiString("Not Found"),
                ["status"] = new OpenApiInteger(404),
                ["detail"] = new OpenApiString("Listing with id 'f47ac10b-58cc-4372-a567-0e02b2c3d479' was not found.")
            };
    }
}