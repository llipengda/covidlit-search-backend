using System.Security.Claims;
using System.Text;
using CovidLitSearch.Services;
using CovidLitSearch.Services.Interface;
using CovidLitSearch.Utilities.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CovidLitSearch.Utilities;

public static class Extensions
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddSingleton<IAuthorizationMiddlewareResultHandler, AuthorizationResultHandler>();
        services.AddScoped<IArticleService, ArticleService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthorService, AuthorService>();
        services.AddScoped<IJournalService, JournalService>();
        services.AddScoped<IHistoryService, HistoryService>();
        services.AddScoped<ICollectService, CollectService>();
        services.AddScoped<ISubscribeService, SubscribeService>();
        services.AddScoped<ICodeService, CodeService>();
    }

    public static void SetupSwagger(this SwaggerGenOptions option)
    {
        option.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "CovidLitSearch.xml"));
        option.DocumentFilter<EnumDocumentFilter>();
        option.OperationFilter<OpenApiOperationFilter>();
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

    public static void AddJwtAuthentication(
        this IServiceCollection services,
        ConfigurationManager configuration
    )
    {
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(option =>
                option.TokenValidationParameters = new()
                {
                    ValidateIssuer = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = configuration["Jwt:Audience"],
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]!)
                    )
                }
            );
    }

    /// <summary>
    /// Try to get name identifier from claims.
    /// If the name identifier does not exist, return null.
    /// </summary>
    /// <param name="claims"></param>
    /// <returns></returns>
    public static int? TryGetId(this ClaimsPrincipal claims)
    {
        var id = claims.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.TryParse(id, out var result) ? result : null;
    }

    /// <summary>
    /// Get name identifier from claims.
    /// Assume that the name identifier exists.
    /// Otherwise, throw an exception.
    /// </summary>
    /// <param name="claims"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException">Name identifier not found in claims</exception>
    public static int GetId(this ClaimsPrincipal claims)
    {
        var id = claims.FindFirstValue(ClaimTypes.NameIdentifier)
                 ?? throw new InvalidOperationException("Name identifier not found in claims");
        return int.Parse(id);
    }
}