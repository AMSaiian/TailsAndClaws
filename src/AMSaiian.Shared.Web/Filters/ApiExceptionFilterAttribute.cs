using AMSaiian.Shared.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AMSaiian.Shared.Web.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
{
    protected readonly IDictionary<Type, Action<Exception, ExceptionContext>> _exceptionHandlers;

    public ApiExceptionFilterAttribute()
    {
        _exceptionHandlers = new Dictionary<Type, Action<Exception, ExceptionContext>>
        {
            { typeof(ValidationException), HandleValidationException },
            { typeof(ConflictException), HandleConflictException }
        };
    }

    public override void OnException(ExceptionContext context)
    {
        HandleException(context);

        base.OnException(context);
    }

    protected virtual void HandleException(ExceptionContext context)
    {
        Exception? exception = context.Exception;

        if (_exceptionHandlers.TryGetValue(exception.GetType(),
                                           out Action<Exception, ExceptionContext>? handler)
         || ((exception = context.Exception.InnerException) is not null
          && _exceptionHandlers.TryGetValue(exception.GetType(),
                                            out handler)))
        {
            handler(exception, context);
            context.ExceptionHandled = true;
        }
    }

    protected virtual void HandleValidationException(Exception exception, ExceptionContext context)
    {
        var castedException = (ValidationException)exception;

        var details = new ValidationProblemDetails(castedException.Errors)
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
        };

        context.Result = new BadRequestObjectResult(details);
    }

    protected virtual void HandleConflictException(Exception exception, ExceptionContext context)
    {
        var castedException = (ConflictException)exception;

        var details = new ProblemDetails
        {
            Status = StatusCodes.Status409Conflict,
            Title = "Conflict occured during processing request",
            Detail = castedException.Message,
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.8"
        };

        context.Result = new ObjectResult(details)
        {
            StatusCode = StatusCodes.Status409Conflict
        };
    }
}
