using Microsoft.AspNetCore.JsonPatch;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MuseumExhibits.API
{
    public static class SwaggerConfiguration
    {
        public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Museum Exhibits API",
                    Version = "v1",
                    Description = "API for managing museum exhibits"
                });

                c.MapType<IFormFile>(() => new OpenApiSchema { Type = "string", Format = "binary" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });

                c.OperationFilter<JsonPatchDocumentFilter>();
                c.DocumentFilter<RemoveOperationSchemaFilter>();
            });

            return services;
        }

        public class JsonPatchDocumentFilter : IOperationFilter
        {
            public void Apply(OpenApiOperation operation, OperationFilterContext context)
            {
                var isJsonPatch = context.MethodInfo
                    .GetParameters()
                    .Any(p => p.ParameterType.IsGenericType &&
                              p.ParameterType.GetGenericTypeDefinition() == typeof(JsonPatchDocument<>));

                if (isJsonPatch && operation.RequestBody?.Content?.ContainsKey("application/json-patch+json") == true)
                {
                    operation.RequestBody.Content["application/json-patch+json"].Schema = new OpenApiSchema
                    {
                        Type = "array",
                        Items = new OpenApiSchema
                        {
                            Type = "object",
                            Properties = new Dictionary<string, OpenApiSchema>
                            {
                                ["op"] = new OpenApiSchema { Type = "string", Example = new OpenApiString("replace") },
                                ["path"] = new OpenApiSchema { Type = "string", Example = new OpenApiString("/name") },
                                ["value"] = new OpenApiSchema { Type = "string", Example = new OpenApiString("NewName") }
                            }
                        }
                    };
                }
            }
        }

        private class RemoveOperationSchemaFilter : IDocumentFilter
        {
            public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
            {
                var keysToRemove = context.SchemaRepository.Schemas
                    .Where(kvp =>
                        kvp.Key.Equals("Operation", StringComparison.OrdinalIgnoreCase) ||
                        kvp.Key.Equals("OperationType", StringComparison.OrdinalIgnoreCase))
                    .Select(kvp => kvp.Key)
                    .ToList();

                foreach (var key in keysToRemove)
                {
                    context.SchemaRepository.Schemas.Remove(key);
                }
            }
        }
    }
}