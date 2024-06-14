using System.Security.Claims;
using System.Text;
using CovidLitSearch.Services;
using CovidLitSearch.Services.Interface;
using CovidLitSearch.Utilities.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CovidLitSearch.Utilities;

public static class Extensions
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IArticleService, ArticleService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthorService, AuthorService>();
        services.AddScoped<IJournalService, JournalService>();
        services.AddScoped<IHistoryService, HistoryService>();
        services.AddScoped<ICollectService, CollectService>();
        services.AddScoped<ISubscribeService, SubscribeService>();
        services.AddScoped<IVerifyCodeService, VerifyCodeService>();
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

    public static int GetId(this ClaimsPrincipal claims)
    {
        var id = claims.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.Parse(id!);
    }
}
