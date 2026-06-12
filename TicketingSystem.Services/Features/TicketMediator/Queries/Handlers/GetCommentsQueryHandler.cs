using AutoMapper;
using MediatR;
using TicketingSystem.Repository.UnitOfWork.Abstraction;
using TicketingSystem.Services.DTOs.CommentDtos;
using TicketingSystem.Services.Helpers;

public class GetCommentsQueryHandler
    : IRequestHandler<GetCommentsQuery,
        PagedResult<CommentDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCommentsQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PagedResult<CommentDto>> Handle(
        GetCommentsQuery request,
        CancellationToken cancellationToken)
    {
        var query = _unitOfWork.TicketsComments
            .GetByTicketId(request.TicketId);

        return await PaginationHelper
            .ToPagedResultAsync<TicketsComment, CommentDto>(
                query,
                request.Pagination,
                _mapper);
    }
}