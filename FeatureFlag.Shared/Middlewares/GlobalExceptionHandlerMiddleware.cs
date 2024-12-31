using System.Net;
using System.Text.Json;
using FeatureFlag.Shared.Helpers;
using Microsoft.AspNetCore.Http;

namespace FeatureFlag.Shared.Middlewares;

public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public GlobalExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (FeatureFlagAppExeception ex)
        {
            await HandleFeatureFlagExceptionAsync(context, ex);
        }
        catch (Exception ex)
        {
            await HandleGenericExceptionAsync(context, ex);
        }
    }

    private static Task HandleFeatureFlagExceptionAsync(HttpContext context, FeatureFlagAppExeception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        var response = RespostaBase.Erro(exception.Message, (int)exception.CodigoErro);
        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }

    private static Task HandleGenericExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var response = RespostaBase.Erro("Ocorreu um erro inesperado. Entre em contato com o suporte.");
        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}