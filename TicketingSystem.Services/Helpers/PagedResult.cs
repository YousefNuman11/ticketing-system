using System.Collections;

namespace TicketingSystem.Services.Helpers
{
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
