using System.Net;
using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Tienda.Domain.Exceptions;

namespace Tienda.Api.Middlewares;

public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

    public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
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
            _logger.LogError(ex, "Ocurrió un error no controlado: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/problem+json";

        var problemDetails = new ProblemDetails();

        switch (exception)
        {
            case ValidationException validationEx:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                problemDetails.Status = context.Response.StatusCode;
                problemDetails.Title = "Error de Validación de Entrada";
                problemDetails.Detail = "Uno o más campos enviados no cumplen con la estructura válida.";
                problemDetails.Extensions["errors"] = validationEx.Errors
                    .Select(e => new { Field = e.PropertyName, Error = e.ErrorMessage });
                break;

            case InsufficientStockException domainEx:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                problemDetails.Status = context.Response.StatusCode;
                problemDetails.Title = "Regla de Negocio Violada";
                problemDetails.Detail = domainEx.Message;
                break;

            default:
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                problemDetails.Status = context.Response.StatusCode;
                problemDetails.Title = "Error Interno del Servidor";
                problemDetails.Detail = "Ocurrió un error inesperado al procesar la solicitud.";
                break;
        }

        string jsonResponse = JsonSerializer.Serialize(problemDetails);
        return context.Response.WriteAsync(jsonResponse);
    }
}