using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using PetManager.Application.Exceptions;
using PetManager.Api.Models;

namespace PetManager.Api.Middleware;

public class ApiExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ApiExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
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

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        string status = "500";
        int httpStatus = (int)HttpStatusCode.InternalServerError;
        string message = "Internal server error";

        if (exception is UserNotFoundException)
        {
            status = "404";
            httpStatus = (int)HttpStatusCode.NotFound;
            message = exception.Message;
        }
        else
        {
            message = exception.Message;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = httpStatus;

        var respType = typeof(ApiResponse<object>);
        var resp = ApiResponse<object>.Error(status, message, null);
        var json = JsonSerializer.Serialize(resp);
        return context.Response.WriteAsync(json);
    }
}
