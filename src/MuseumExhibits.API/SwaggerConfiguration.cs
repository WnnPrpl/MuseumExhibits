using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text.Json.Nodes;

namespace MuseumExhibits.API;

public static class SwaggerConfiguration
{
    public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title       = "Museum Exhibits API",
                Version     = "v1",
                Description = "API for managing museum exhibits"
            });

            // Binary file upload schema
            c.MapType<IFormFile>(() => new OpenApiSchema
            {
                Type   = JsonSchemaType.String,
                Format = "binary"
            });

            // JWT Bearer auth
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header. Example: \"Bearer {token}\"",
                Name        = "Authorization",
                In          = ParameterLocation.Header,
                Type        = SecuritySchemeType.ApiKey
            });

            c.AddSecurityRequirement(_ => new OpenApiSecurityRequirement
            {
                { new OpenApiSecuritySchemeReference("Bearer"), [] }
            });
        });

        return services;
    }
}
