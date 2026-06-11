using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TicketingSystem.Services.Helpers;

namespace TicketingSystem.Services.Helpers
{
    public static class PaginationHelper
    {
        public static async Task<PagedResult<TDto>> ToPagedResultAsync<TEntity, TDto>(
            IQueryable<TEntity> query,
            PaginationDto pagination,
            IMapper mapper)
        {
            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();

            return new PagedResult<TDto>
            {
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pagination.PageSize),
                Items = mapper.Map<List<TDto>>(items)
            };
        }
    }
}