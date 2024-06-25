using CovidLitSearch.Models.Common;
using CovidLitSearch.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;

namespace CovidLitSearch.Utilities;

public class AuthorizationResultHandler : IAuthorizationMiddlewareResultHandler
{
    private readonly AuthorizationMiddlewareResultHandler _defaultHandler = new();

    public async Task HandleAsync(RequestDelegate next, HttpContext context, AuthorizationPolicy policy,
        PolicyAuthorizationResult authorizeResult)
    {
        if (authorizeResult.Challenged)
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsJsonAsync(
                new Error(ErrorCode.InvalidToken)
            );
        }
        else if (authorizeResult.Forbidden)
        {
            context.Response.StatusCode = 403;
            await context.Response.WriteAsJsonAsync(
                new Error(ErrorCode.NoPermission)
            );
        }
        else
        {
            await _defaultHandler.HandleAsync(next, context, policy, authorizeResult);
        }
    }
}