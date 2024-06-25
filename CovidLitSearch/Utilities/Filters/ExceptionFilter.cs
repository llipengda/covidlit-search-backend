using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CovidLitSearch.Utilities.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        context.Result = new ObjectResult(
            new
            {
                message = context.Exception.Message,
                stackTrace = context.Exception.StackTrace
            })
        {
            StatusCode = 500
        };
    }
}