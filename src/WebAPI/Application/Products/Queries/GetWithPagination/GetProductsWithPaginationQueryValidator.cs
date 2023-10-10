using FluentValidation;

namespace Application.Products.Queries.GetWithPagination;


public class GetProdutcsWithPaginationQueryValidator : AbstractValidator<GetProdutcsWithPaginationQuery>
{
    public GetProdutcsWithPaginationQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1)
            .WithMessage("El número de página debe ser mayor o igual a uno.");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .WithMessage("El tamaño de la página debe ser mayor o igual a uno.");
    }
}

