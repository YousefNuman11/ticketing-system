namespace TicketingSystem.API.Common
{
    /// <summary>
    /// Standard envelope returned by every endpoint.
    /// </summary>
    public class ApiResponse
    {
        public bool Success { get; init; }
        public int StatusCode { get; init; }
        public string? Message { get; init; }
        public object? Data { get; init; }
        public PaginationMeta? Pagination { get; init; }
        public IEnumerable<string>? Errors { get; init; }

        public static ApiResponse Ok(
            object? data,
            int statusCode = StatusCodes.Status200OK,
            string? message = null,
            PaginationMeta? pagination = null) => new()
            {
                Success = true,
                StatusCode = statusCode,
                Message = message ?? "Request successful",
                Data = data,
                Pagination = pagination
            };

        public static ApiResponse Fail(
            int statusCode,
            string message,
            IEnumerable<string>? errors = null) => new()
            {
                Success = false,
                StatusCode = statusCode,
                Message = message,
                Errors = errors
            };
    }

    public class PaginationMeta
    {
        public int PageNumber { get; init; }
        public int PageSize { get; init; }
        public int TotalCount { get; init; }
        public int TotalPages { get; init; }

        public PaginationMeta(int pageNumber, int pageSize, int totalCount, int totalPages)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalCount = totalCount;
            TotalPages = totalPages;
        }
    }
}
