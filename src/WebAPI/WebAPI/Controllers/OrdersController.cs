using Application.Common.Dtos.Paginated;
using Application.Orders.Commands.Create;
using Application.Orders.Commands.TakeOrder;
using Application.Orders.Queries.GetWithPagination;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Controllers;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Web.API.Controllers;

[Authorize]
[Route("api/[controller]")]
public class OrdersController : ApiController
{
    private readonly ISender _mediator;

    public OrdersController(ISender mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    /// <summary>
    /// Create order. Default Order Status is Peding.
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrderCommand command)
    {
        var createResult = await _mediator.Send(command);
        return createResult.Match(
            value => Ok(value),
            errors => Problem(errors)
        );
    }

    /// <summary>
    /// Takes the order for an employee and places it in InProgress status.
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPatch]
    public async Task<IActionResult> Take([FromBody] TakeOrderCommand command)
    {
        var createResult = await _mediator.Send(command);
        return createResult.Match(
            value => Ok(value),
            errors => Problem(errors)
        );
    }

    /// <summary>
    /// Get orders and details with pagination.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="query"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] GetOrdersWithPaginationQuery query)
    {
        var createResult = await _mediator.Send(query);
        return createResult.Match(
            value => Ok(value),
            errors => Problem(errors)
        );
    }

}