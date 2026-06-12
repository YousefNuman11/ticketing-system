using AutoMapper;
using MediatR;
using TicketingSystem.Repository.Models;
using TicketingSystem.Repository.UnitOfWork.Abstraction;
using TicketingSystem.Services.DTOs.TicketAttachmentDto;
using TicketingSystem.Services.Helpers;

public class GetAttachmentsQueryHandler
    : IRequestHandler<GetAttachmentsQuery, PagedResult<AttachmentDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAttachmentsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PagedResult<AttachmentDto>> Handle(
        GetAttachmentsQuery request,
        CancellationToken cancellationToken)
    {
        var query = _unitOfWork.TicketAttachments
            .GetByTicketId(request.TicketId);

        return await PaginationHelper
            .ToPagedResultAsync<TicketAttachment, AttachmentDto>(
                query,
                request.Pagination,
                _mapper);
    }
}