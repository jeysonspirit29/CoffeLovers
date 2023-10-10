using Application.Common.Dtos.Paginated;
using Application.Orders.Commands.Create;
using Application.Orders.Queries.GetWithPagination;
using Application.Products.Queries.GetWithPagination;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Controllers;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Web.API.Controllers;

[Authorize]
[Route("api/[controller]")]
public class ProductsController : ApiController
{
    private readonly ISender _mediator;

    public ProductsController(ISender mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }


    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] GetProdutcsWithPaginationQuery query)
    {
        var createResult = await _mediator.Send(query);
        return createResult.Match(
            value => Ok(value),
            errors => Problem(errors)
        );
    }

}