using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using TodoList.Api.Middlewares;
using TodoList.Api.Models;

namespace TodoList.Api.Extensions;

public static class ExceptionMiddlewareExtensions
{
    // public static void UseGlobalExceptionHandler(this WebApplication app)
    // {
    //     app.UseExceptionHandler(appError =>
    //     {
    //         appError.Run(async context =>
    //         {
    //             context.Response.ContentType = "application/json";

    //             var errorFeature = context.Features.Get<IExceptionHandlerFeature>();
    //             if (errorFeature != null)
    //             {
    //                 await context.Response.WriteAsync(new ErrorResponse
    //                 {
    //                     StatusCode = (HttpStatusCode)context.Response.StatusCode,
    //                     Message = errorFeature.Error.Message
    //                 }.ToJsonString());
    //             }
    //         });
    //     });
    // }
    
    public static WebApplication UseGlobalExceptionHandler(this WebApplication app)
    {
        app.UseMiddleware<GlobalExceptionMiddleware>();
        return app;
    }
}