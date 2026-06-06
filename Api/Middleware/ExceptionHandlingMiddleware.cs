using System.ComponentModel.DataAnnotations;
using System.Net;
using Domain.Shared.Exceptions;

namespace Api.Middleware;

public sealed class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(context, exception);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, code, message, errors) = exception switch
        {
            ValidationException validationException => (
                HttpStatusCode.BadRequest,
                "validation_error",
                "Validation failed",
                new Dictionary<string, string[]> { ["validation"] = [validationException.Message] }),
            UnauthorizedAccessException => (HttpStatusCode.Unauthorized, "unauthorized", exception.Message, null),
            AccountDisabledException => (HttpStatusCode.Forbidden, "account_disabled", exception.Message, null),
            ForbiddenAccessException => (HttpStatusCode.Forbidden, "forbidden", exception.Message, null),
            EntityNotFoundException => (HttpStatusCode.NotFound, "not_found", exception.Message, null),
            BusinessRuleException => (HttpStatusCode.Conflict, "business_rule", exception.Message, null),
            _ => (HttpStatusCode.InternalServerError, "server_error", "An unexpected error occurred.", null)
        };

        if (statusCode == HttpStatusCode.InternalServerError)
        {
            logger.LogError(exception, "Unhandled exception");
        }

        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json";

        await context.Response.WriteAsJsonAsync(new ErrorResponse(code, message, errors));
    }

    private sealed record ErrorResponse(
        string Code,
        string Message,
        IReadOnlyDictionary<string, string[]>? Errors);
}
