using System.Net;
using System.Text.Json;
using EngConnect.BuildingBlock.Contracts.Shared;
using EngConnect.BuildingBlock.Domain.DomainErrors;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace EngConnect.BuildingBlock.Presentation.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        _logger.LogError(exception, "An unhandled exception occurred");

        var errorResponse = Result.Failure(HttpStatusCode.InternalServerError, CommonErrors.InternalServerError());

        switch (exception)
        {
            case ValidationException validationException:
            {
                var errors = validationException.Errors
                    .Select(e => e.ErrorMessage)
                    .ToList();

                errorResponse = Result.Failure(HttpStatusCode.BadRequest, new Error("Validation.Error", string.Join(", ", errors)));
                break;
            }
            case SecurityTokenException:
                errorResponse = Result.Failure(HttpStatusCode.Unauthorized, new Error("Auth.InvalidToken", "The provided token is invalid or expired"));
                break;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)errorResponse.HttpStatusCode;

        await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
    }
}

// Extension method for startup
public static class ExceptionHandlingMiddlewareExtensions
{
    public static void UseErrorHandling(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}