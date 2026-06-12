using MediatR;

namespace TicketingSystem.API.Features.Dashboard.Queries.GetTopEmployees
{
    public record GetTopEmployeesQuery : IRequest<object>;
}