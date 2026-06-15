using System.Collections;

namespace TicketingSystem.Services.Helpers
{
    /// <summary>
    /// Non-generic view over a paged result so the API layer can split
    /// items (data) from pagination metadata without reflection.
    /// </summary>
    public interface IPagedResult
    {
        int PageNumber { get; }
        int PageSize { get; }
        int TotalCount { get; }
        int TotalPages { get; }
        IEnumerable ItemsObject { get; }
    }

    public class PagedResult<T> : IPagedResult
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public List<T> Items { get; set; } = new();

        IEnumerable IPagedResult.ItemsObject => Items;
    }
}
