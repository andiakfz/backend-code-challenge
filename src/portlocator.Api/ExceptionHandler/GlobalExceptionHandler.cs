using Microsoft.AspNetCore.Diagnostics;
using portlocator.Shared;

namespace portlocator.Api.ExceptionHandler
{
    public sealed class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _log;
        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> log)
        {
            _log = log;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            _log.LogError(exception, "System Failure: Unhandled Exception");

            var response = Result.Failure<object>(null,exception.Message);

            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

            await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);

            return true;
        }
    }
}
