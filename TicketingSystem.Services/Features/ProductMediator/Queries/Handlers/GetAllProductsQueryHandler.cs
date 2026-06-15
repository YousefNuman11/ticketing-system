using AutoMapper;
using MediatR;
using TicketingSystem.Repository.Models;
using TicketingSystem.Repository.UnitOfWork.Abstraction;
using TicketingSystem.Services.Features.ProductMediator.Contracts;
using TicketingSystem.Services.Helpers;

namespace TicketingSystem.Services.Features.ProductMediator.Queries
{
    public class GetAllProductsQueryHandler
        : IRequestHandler<GetAllProductsQuery, PagedResult<ProductDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllProductsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResult<ProductDto>> Handle(
            GetAllProductsQuery request,
            CancellationToken cancellationToken)
        {
            var query = _unitOfWork.Products
                .GetProductsQuery()
                .Where(p => p.IsActive);

            return await PaginationHelper
                .ToPagedResultAsync<Product, ProductDto>(query, request.Pagination, _mapper);
        }
    }
}
