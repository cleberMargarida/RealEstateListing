using System.Reflection;

namespace RealEstateListing.API.Extensions;

/// <summary>
/// Extension methods for configuring Web API services.
/// </summary>
public static class WebApiExtensions
{
    /// <summary>
    /// Adds and configures Swagger/OpenAPI documentation for the API.
    /// </summary>
    /// <param name="services">The service collection to add Swagger to.</param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new() { Title = "Real Estate Listing API", Version = "v1" });

            // Include XML comments from all projects
            var xmlFiles = new[]
            {
                $"{Assembly.GetExecutingAssembly().GetName().Name}.xml",
                "RealEstateListing.Application.xml",
                "RealEstateListing.Domain.xml"
            };

            foreach (var xmlFile in xmlFiles)
            {
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    c.IncludeXmlComments(xmlPath);
                }
            }
        });

        return services;
    }
}
