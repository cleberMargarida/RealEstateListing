using System;
using System.Collections.Generic;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using RealEstateListing.API.ViewModels;
using RealEstateListing.Domain.ValueObjects;

namespace RealEstateListing.API.Swagger
{
    /// <summary>
    /// Adds concrete examples to OpenAPI schemas used by the API so Swagger UI shows valid, copy-pastable payloads.
    /// Targets: <see cref="CreateListingRequest"/>, <see cref="ListingResponse"/>, <see cref="Money"/>, <see cref="Address"/> and enums.
    /// </summary>
    internal class SwaggerSchemaExamplesFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            var type = context.Type;

            if (type == typeof(Money))
            {
                schema.Type = "object";
                schema.Properties ??= new Dictionary<string, OpenApiSchema>(StringComparer.OrdinalIgnoreCase);
                schema.Properties["amount"] = new OpenApiSchema { Type = "number", Format = "decimal", Example = new OpenApiDouble(350000.00) };
                schema.Properties["currency"] = new OpenApiSchema { Type = "integer", Format = "int32", Example = new OpenApiInteger((int)CurrencyCode.USD) };
                schema.Required = new HashSet<string> { "amount", "currency" };
                schema.Example = new OpenApiObject
                {
                    ["amount"] = new OpenApiDouble(350000.00),
                    ["currency"] = new OpenApiInteger((int)CurrencyCode.USD)
                };

                return;
            }

            if (type == typeof(Address))
            {
                schema.Example = new OpenApiObject
                {
                    ["street"] = new OpenApiString("123 Maple Ave"),
                    ["city"] = new OpenApiString("Springfield"),
                    ["state"] = new OpenApiString("IL"),
                    ["zipCode"] = new OpenApiString("62704")
                };

                return;
            }

            if (type == typeof(CreateListingRequest) || type.Name == "UpdateListingRequest")
            {
                // Create/Update request example (no id / timestamps)
                schema.Example = new OpenApiObject
                {
                    ["title"] = new OpenApiString("Light-filled 3‑bed near downtown"),
                    ["price"] = new OpenApiObject
                    {
                        ["amount"] = new OpenApiDouble(350000.00),
                        ["currency"] = new OpenApiInteger((int)CurrencyCode.USD)
                    },
                    ["description"] = new OpenApiString("Recently renovated, open-plan living, 2 baths, garage."),
                    ["address"] = new OpenApiObject
                    {
                        ["street"] = new OpenApiString("123 Maple Ave"),
                        ["city"] = new OpenApiString("Springfield"),
                        ["state"] = new OpenApiString("IL"),
                        ["zipCode"] = new OpenApiString("62704")
                    }
                };

                return;
            }

            if (type == typeof(ListingResponse))
            {
                schema.Example = new OpenApiObject
                {
                    ["id"] = new OpenApiString("f47ac10b-58cc-4372-a567-0e02b2c3d479"),
                    ["title"] = new OpenApiString("Light-filled 3‑bed near downtown"),
                    ["price"] = new OpenApiObject
                    {
                        ["amount"] = new OpenApiDouble(350000.00),
                        ["currency"] = new OpenApiInteger((int)CurrencyCode.USD)
                    },
                    ["description"] = new OpenApiString("Recently renovated, open-plan living, 2 baths, garage."),
                    ["address"] = new OpenApiObject
                    {
                        ["street"] = new OpenApiString("123 Maple Ave"),
                        ["city"] = new OpenApiString("Springfield"),
                        ["state"] = new OpenApiString("IL"),
                        ["zipCode"] = new OpenApiString("62704")
                    },
                    ["status"] = new OpenApiInteger((int)ListingStatus.Published),
                    ["createdAt"] = new OpenApiString(DateTime.UtcNow.ToString("o")),
                    ["updatedAt"] = new OpenApiString(DateTime.UtcNow.ToString("o"))
                };

                return;
            }

            // Provide numeric examples for enums (project uses numeric enum serialization by default)
            if (type.IsEnum)
            {
                if (type == typeof(ListingStatus))
                    schema.Example = new OpenApiInteger((int)ListingStatus.Published);

                if (type == typeof(CurrencyCode))
                    schema.Example = new OpenApiInteger((int)CurrencyCode.USD);
            }
        }
    }
}