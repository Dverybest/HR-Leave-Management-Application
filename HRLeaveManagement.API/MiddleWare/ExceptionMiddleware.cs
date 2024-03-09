using System.Net;
using System.Text.Json.Serialization;
using HRLeaveManagement.API.MiddleWare.Models;
using HRLeaveManagement.Application.Contracts.Logging;
using HRLeaveManagement.Application.Exceptions;
using Newtonsoft.Json;

namespace HRLeaveManagement.API.MiddleWare;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext,IAppLogger<ExceptionMiddleware> logger)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex,logger);
        }
    }

    private async Task HandleExceptionAsync(HttpContext httpContext, Exception ex,IAppLogger<ExceptionMiddleware>logger)
    {
        HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
        CustomProblemDetails problem = new();

        switch (ex)
        {
            case BadRequestException badRequestException:
                statusCode = HttpStatusCode.BadRequest;
                problem = new CustomProblemDetails
                {
                    Title = badRequestException.Message,
                    Status = (int)statusCode,
                    Detail = badRequestException.InnerException?.Message,
                    Type = nameof(BadRequestException),
                    Errors = badRequestException.ValidationErrors
                };
                break;
            case NotFoundException notFoundException:
                statusCode = HttpStatusCode.NotFound;
                problem = new CustomProblemDetails
                {
                    Title = notFoundException.Message,
                    Status = (int)statusCode,
                    Detail = notFoundException.InnerException?.Message,
                    Type = nameof(BadRequestException),
                };
                break;
            default:
                problem = new CustomProblemDetails
                {
                    Title = ex.Message,
                    Status = (int)statusCode,
                    Detail = ex.StackTrace,
                    Type = nameof(HttpStatusCode.InternalServerError),
                };
                break;
        }

        httpContext.Response.StatusCode = (int)statusCode;
        var logMessage = JsonConvert.SerializeObject(problem);
        logger.LogError(logMessage);
        await httpContext.Response.WriteAsJsonAsync(problem);
    }
}
