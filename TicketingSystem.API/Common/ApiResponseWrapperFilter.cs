using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TicketingSystem.Services.Helpers;

namespace TicketingSystem.API.Common
{
    /// <summary>
    /// Wraps every controller result in <see cref="ApiResponse"/> and lifts
    /// paginated payloads into a separate pagination block.
    /// </summary>
    public sealed class ApiResponseWrapperFilter : IAsyncResultFilter
    {
        public async Task OnResultExecutionAsync(
            ResultExecutingContext context,
            ResultExecutionDelegate next)
        {
            switch (context.Result)
            {
                // Already wrapped (e.g. model-validation factory) -> leave as-is.
                case ObjectResult o when o.Value is ApiResponse:
                    break;

                case ObjectResult objectResult:
                {
                    var statusCode = objectResult.StatusCode ?? StatusCodes.Status200OK;
                    var success = statusCode is >= 200 and < 300;

                    object? data = objectResult.Value;
                    PaginationMeta? pagination = null;

                    if (data is IPagedResult paged)
                    {
                        pagination = new PaginationMeta(
                            paged.PageNumber, paged.PageSize,
                            paged.TotalCount, paged.TotalPages);
                        data = paged.ItemsObject;
                    }

                    var body = success
                        ? ApiResponse.Ok(data, statusCode, pagination: pagination)
                        : ApiResponse.Fail(statusCode, DefaultMessage(statusCode));

                    context.Result = new ObjectResult(body) { StatusCode = statusCode };
                    break;
                }

                // Empty results such as NotFound()/Ok() with no body.
                case StatusCodeResult statusResult:
                {
                    var statusCode = statusResult.StatusCode;
                    var success = statusCode is >= 200 and < 300;

                    var body = success
                        ? ApiResponse.Ok(null, statusCode)
                        : ApiResponse.Fail(statusCode, DefaultMessage(statusCode));

                    context.Result = new ObjectResult(body) { StatusCode = statusCode };
                    break;
                }
            }

            await next();
        }

        private static string DefaultMessage(int statusCode) => statusCode switch
        {
            StatusCodes.Status400BadRequest => "Bad request",
            StatusCodes.Status401Unauthorized => "Unauthorized",
            StatusCodes.Status403Forbidden => "Forbidden",
            StatusCodes.Status404NotFound => "Resource not found",
            StatusCodes.Status409Conflict => "Conflict",
            _ => "Request failed"
        };
    }
}
