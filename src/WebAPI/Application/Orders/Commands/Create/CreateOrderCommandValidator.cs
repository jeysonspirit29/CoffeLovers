using Application.Orders.Commands.Create;
using FluentValidation;

namespace Application.Orders.Commands.Create;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(r => r.TaxId)
             .GreaterThanOrEqualTo(1)
             .WithMessage("El impuesto no es válido.");

        RuleFor(r => r.Items)
             .Must(x => x != null && x.Any())
             .WithMessage("La orden debe tener al menos un producto.")
             .Must(x => x.All(p => p.ProductId > 0))
             .WithMessage("El producto de la orden no es válido.")
             .Must(x => x.All(p => p.Quantity > 0))
             .WithMessage("La cantidad de la orden debe ser mayor cero.");
    }
}