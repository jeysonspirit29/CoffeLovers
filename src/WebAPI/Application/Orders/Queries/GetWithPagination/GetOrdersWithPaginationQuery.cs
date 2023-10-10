using Application.Common.Dtos.Paginated;
using Application.Common.Interfaces;
using Application.Common.Mappings;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Contants;
using Domain.Domains.Orders;
using Domain.Domains.OrderStatuses;
using Domain.Domains.Products;
using Domain.Domains.Users;
using Domain.Enums;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Orders.Queries.GetWithPagination;


public record GetOrdersWithPaginationQuery : IRequest<ErrorOr<PaginatedList<OrderDto>>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetOrdersWithPaginationQueryHandler : IRequestHandler<GetOrdersWithPaginationQuery, ErrorOr<PaginatedList<OrderDto>>>
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;
    private readonly IUser _user;
    private readonly IMapper _mapper;

    public GetOrdersWithPaginationQueryHandler(IApplicationDbContext context,
                                               IMapper mapper,
                                               IUser user,
                                               IIdentityService identityService)
    {
        _context = context;
        _mapper = mapper;
        _user = user;
        _identityService = identityService;
    }

    public async Task<ErrorOr<PaginatedList<OrderDto>>> Handle(GetOrdersWithPaginationQuery request, CancellationToken cancellationToken)
    {
        string userId = _user.Id;
        string rol = _user.Role;

        switch (rol)
        {
            case Roles.User:
                return await GetOrdersByUser(request, userId);
            case Roles.Employee:
                return await GetOrdersByEmployee(request, userId);
            case Roles.Supervisor:
                return await GetOrdersBySupervisor(request, userId);
            case Roles.Administrator:
                return await GetOrdersByAdministrator(request);
            default:
                return new PaginatedList<OrderDto>(new List<OrderDto>(), 1, 1, 1);
        }
    }

    private async Task<PaginatedList<OrderDto>> GetOrdersByAdministrator(GetOrdersWithPaginationQuery request)
    {
        return await GetOrdersQueryBase()
                .OrderBy(x => x.Id)
                .ProjectTo<OrderDto>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
    private async Task<PaginatedList<OrderDto>> GetOrdersBySupervisor(GetOrdersWithPaginationQuery request, string userId)
    {
        var userIds = await _identityService.GetUserIdsBySuperior(userId);
        return await GetOrdersQueryBase()
               .Where(x => userIds.Contains(x.AttentionUserId))
               .OrderBy(x => x.Id)
               .ProjectTo<OrderDto>(_mapper.ConfigurationProvider)
               .PaginatedListAsync(request.PageNumber, request.PageSize);
    }

    private async Task<PaginatedList<OrderDto>> GetOrdersByEmployee(GetOrdersWithPaginationQuery request, string userId)
    {
        return await GetOrdersQueryBase()
               .Where(x => x.AttentionUserId == userId || x.OrderStatusId == (int)OrderStatuses.Pending)
               .OrderBy(x => x.Id)
               .ProjectTo<OrderDto>(_mapper.ConfigurationProvider)
               .PaginatedListAsync(request.PageNumber, request.PageSize);
    }

    private async Task<PaginatedList<OrderDto>> GetOrdersByUser(GetOrdersWithPaginationQuery request, string userId)
    {
        return await GetOrdersQueryBase()
                .Where(x => x.RequestingUserId == userId)
                .OrderBy(x => x.Id)
                .ProjectTo<OrderDto>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(request.PageNumber, request.PageSize);
    }

    private IIncludableQueryable<Order, Product> GetOrdersQueryBase()
    {
        return _context.Orders
                                .Include(x => x.OrderStatus)
                                .Include(x => x.RequestingUser)
                                .Include(x => x.AttentionUser)
                                .Include(x => x.OrderDetails)
                                .ThenInclude(x => x.Product);
    }
}