using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CovidLitSearch.Utilities.Filters;

public class LoggingFilter(ILogger<LoggingFilter> logger) : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        var method = context.HttpContext.Request.Method;
        var path = context.HttpContext.Request.Path;
        var query = context.HttpContext.Request.QueryString;
        logger.LogInformation("[REQUEST] {method} {path}{query}", method, path, query);
    }

    public void OnActionExecuted(ActionExecutedContext? context)
    {
        var method = context?.HttpContext.Request.Method;
        var path = context?.HttpContext.Request.Path;
        var result = context?.Result as ObjectResult;
        var status = result?.StatusCode;
        logger.LogInformation(
            "[RESPONSE] {method} {path} {status}",
            method,
            path,
            status
        );
    }
}