using CovidLitSearch.Services;
using CovidLitSearch.Services.Interface;
using CovidLitSearch.Utilities.Filters;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CovidLitSearch.Utilities;

public static class Extensions
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IArticleService, ArticleService>();
    }

    public static void SetupSwagger(this SwaggerGenOptions option)
    {
        option.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "CovidLitSearch.xml"));
        option.DocumentFilter<EnumDocumentFilter>();
        option.AddSecurityRequirement(
            new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Id = "Bearer",
                            Type = ReferenceType.SecurityScheme
                        }
                    },
                    new List<string>()
                }
            }
        );
        option.AddSecurityDefinition(
            "Bearer",
            new OpenApiSecurityScheme
            {
                Description = "Value: Bearer {token}",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey
            }
        );
    }
}
