using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CovidLitSearch.Utilities.Filters;

public class OpenApiOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var requiredScopes = context.MethodInfo
            .GetCustomAttributes(true)
            .OfType<AuthorizeAttribute>()
            .Select(attr => attr.Roles)
            .Distinct().ToList();

        if (!requiredScopes.Any())
        {
            return;
        }
        
        operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
            
        if (requiredScopes.Contains("Admin"))
        {
            operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden" });
        }

        var oAuthScheme = new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
        };

        var scheme = new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Id = "Bearer",
                Type = ReferenceType.SecurityScheme
            }
        };

        operation.Security = new List<OpenApiSecurityRequirement>
        {
            new OpenApiSecurityRequirement
            {
                [ scheme ] = requiredScopes
            }
        };
    }
}