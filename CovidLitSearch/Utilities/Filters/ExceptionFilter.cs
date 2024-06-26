using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CovidLitSearch.Utilities.Filters;

public class ExceptionFilter(ILogger<ExceptionFilter> logger) : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        context.Result = new ObjectResult(
            new
            {
                message = context.Exception.Message,
                stackTrace = context.Exception.StackTrace
            }
        )
        {
            StatusCode = 500
        };
        logger.LogError(context.Exception, context.Exception.Message);
    }
}