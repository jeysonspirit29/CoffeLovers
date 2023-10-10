using Application.Common.Dtos.Paginated;
using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Orders.Queries.GetWithPagination;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Contants;
using Domain.Domains.Orders;
using Domain.Domains.Products;
using Domain.Enums;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Application.Products.Queries.GetWithPagination;


public record GetProdutcsWithPaginationQuery : IRequest<ErrorOr<PaginatedList<ProductDto>>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetProdutcsWithPaginationQueryHandler : IRequestHandler<GetProdutcsWithPaginationQuery, ErrorOr<PaginatedList<ProductDto>>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetProdutcsWithPaginationQueryHandler(IApplicationDbContext context,
                                               IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ErrorOr<PaginatedList<ProductDto>>> Handle(GetProdutcsWithPaginationQuery request, CancellationToken cancellationToken)
    {
        return await _context.Products
                            .OrderBy(x => x.Name)
                            .ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
                            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }

}